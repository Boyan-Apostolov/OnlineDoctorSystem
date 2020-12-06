using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Repositories;
using OnlineDoctorSystem.Services.Data.Consultations;
using OnlineDoctorSystem.Services.Data.Doctors;
using OnlineDoctorSystem.Services.Data.Events;
using OnlineDoctorSystem.Services.Data.Patients;
using OnlineDoctorSystem.Services.Mapping;
using OnlineDoctorSystem.Services.Messaging;
using OnlineDoctorSystem.Web.ViewModels.Consultations;
using Xunit;

namespace OnlineDoctorSystem.Services.Data.Tests
{
    // TODO: Refactor in the future
    public class ConsultationsServiceTests
    {
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;
        private readonly IConsultationsService consultationsService;
        private readonly IEmailSender emailSender;

        private readonly EfDeletableEntityRepository<CalendarEvent> eventsRepository;//
        private readonly EfDeletableEntityRepository<Consultation> consultationsRepository;//
        private readonly EfDeletableEntityRepository<Patient> patientsRepository;//
        private readonly EfDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly EfDeletableEntityRepository<Doctor> doctorsRepository;//

        public ConsultationsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.eventsRepository = new EfDeletableEntityRepository<CalendarEvent>(new ApplicationDbContext(options.Options));
            this.consultationsRepository = new EfDeletableEntityRepository<Consultation>(new ApplicationDbContext(options.Options));
            this.doctorsRepository = new EfDeletableEntityRepository<Doctor>(new ApplicationDbContext(options.Options));
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            this.patientsRepository = new EfDeletableEntityRepository<Patient>(new ApplicationDbContext(options.Options));
            this.emailSender = new SendGridEmailSender("test");

            this.doctorsService = new DoctorsService(
                this.doctorsRepository,
                this.usersRepository,
                this.emailSender,
                this.consultationsRepository,
                this.patientsRepository);

            this.patientsService = new PatientsService(
                this.patientsRepository,
                this.usersRepository);

            this.consultationsService = new ConsultationsService(
                this.doctorsRepository,
                this.consultationsRepository,
                this.patientsRepository,
                this.eventsRepository,
                this.emailSender,
                this.doctorsService,
                this.patientsService);

            AutoMapperConfig.RegisterMappings(
                typeof(ConsultationViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public void Test1()
        {
            var model = new AddConsultationViewModel()
            {
                StartTime = DateTime.Now.AddMinutes(50).TimeOfDay,
                EndTime = DateTime.Now.AddMinutes(-50).TimeOfDay,
                Date = DateTime.Now.AddDays(2),
            };

            var isTimeCorrect = this.consultationsService.CheckIfTimeIsCorrect(model);

            Assert.False(isTimeCorrect);
        }

        [Fact]
        public void Test2()
        {
            var model = new AddConsultationViewModel()
            {
                StartTime = DateTime.Now.AddMinutes(-50).TimeOfDay,
                EndTime = DateTime.Now.AddMinutes(50).TimeOfDay,
                Date = DateTime.Now.AddDays(2),
            };

            var isTimeCorrect = this.consultationsService.CheckIfTimeIsCorrect(model);

            Assert.True(isTimeCorrect);
        }

        [Fact]
        public void Test3()
        {
            var model = new AddConsultationViewModel()
            {
                StartTime = TimeSpan.Parse("15:15:15"),
                EndTime = TimeSpan.Parse("15:15:15"),
                Date = DateTime.Now.AddDays(2),
            };

            var isTimeCorrect = this.consultationsService.CheckIfTimeIsCorrect(model);

            Assert.False(isTimeCorrect);
        }

        [Fact]
        public async Task IncorrectTimeShouldReturnFalseWhenEndTimeIsBeforeStartTime()
        {
            var model = new AddConsultationViewModel()
            {
                StartTime = DateTime.Now.AddHours(3).TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay,
                Date = DateTime.Now.AddDays(2),
            };

            var isAdded = this.consultationsService.CheckIfTimeIsCorrect(model);

            Assert.False(isAdded);
        }

        [Fact]
        public async Task IncorrectTimeShouldReturnFalseWhenStartAndFinishTimeIsTheSame()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = TimeSpan.Parse("15:15:15"),
                EndTime = TimeSpan.Parse("15:15:15"),
                Date = DateTime.Now.AddDays(1),
            };

            var isAdded = await this.consultationsService.AddConsultation(model, patient.Id);

            Assert.False(isAdded);
        }

        [Fact]
        public async Task IncorrectTimeShouldStopTheConsultationFromCreating()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.AddMinutes(40).TimeOfDay,
                Date = DateTime.Now.AddDays(-1),
            };

            var isAdded = await this.consultationsService.AddConsultation(model, patient.Id);

            Assert.False(isAdded);
        }

        [Fact]
        public async Task CorrectTimeShouldReturnTrue()
        {
            var user = new ApplicationUser() { UserName = "test", Email = "test@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user.Id,
                User = user,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddDays(5).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(2),
            };

            var isTimeCorrect = this.consultationsService.CheckIfTimeIsCorrect(model);

            Assert.True(isTimeCorrect);
        }

        [Fact]
        public async Task IncorrectTimeShouldReturnFalseWhenTheDateIsBeforeToday()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.Now.AddHours(5).TimeOfDay,
                EndTime = DateTime.Now.AddMinutes(40).TimeOfDay,
                Date = DateTime.Now.AddDays(-1),
            };

            var isAdded = await this.consultationsService.AddConsultation(model, patient.Id);

            Assert.False(isAdded);
        }

        [Fact]
        public async Task SuccessfullyAddConsultation()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };

            var isAdded = await this.consultationsService.AddConsultation(model, patient.Id);

            Assert.True(isAdded);
        }

        [Fact]
        public async Task GetConsultationsCountShouldReturnCorrectAmount()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultationsCount = this.consultationsService.GetConsultationsCount();

            Assert.Equal(2, consultationsCount);
        }

        [Fact]
        public async Task UpdateConsultationsWhenCompletedShouldUpdateIsActiveToFalse()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.consultationsRepository.All().FirstAsync();
            consultation.Date = DateTime.Today.AddDays(-1);
            this.consultationsRepository.SaveChangesAsync();

            await this.consultationsService.UpdateConsultationsWhenCompleted();

            Assert.False(consultation.IsActive);
        }

        [Fact]
        public async Task MakingConsultationReviewToTrueShouldChangeTheProperty()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.consultationsRepository.All().FirstAsync();

            await this.consultationsService.MakeConsultationReviewedToTrue(consultation.Id);

            Assert.True(consultation.IsReviewed);
        }

        [Fact]
        public async Task SuccessfullyDecliningConsultationShouldUpdateItsProperty()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.consultationsRepository.All().FirstAsync();

            await this.consultationsService.DeclineConsultationAsync(consultation.Id);

            Assert.False(consultation.IsConfirmed);
        }

        [Fact]
        public async Task SuccessfullyApprovingConsultationShouldConfirmTheConsultation()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.consultationsRepository.All().FirstAsync();

            await this.consultationsService.ApproveConsultationAsync(consultation.Id);

            Assert.True(consultation.IsConfirmed);
        }

        [Fact]
        public async Task GettingDoctorsUnconfirmedConsultationsShouldReturnTheCorrectAmountOfThem()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);
            await this.consultationsService.AddConsultation(model, patient.Id);

            var unconfirmedConsultations =
                this.consultationsService.GetDoctorsUnconfirmedConsultations(doctor.Id).Count();

            Assert.Equal(2, unconfirmedConsultations);
        }

        [Fact]
        public async Task GettingDoctorsConsultationsShouldReturnTheCorrectAmountOfThem()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultationsCount =
                this.consultationsService.GetDoctorsConsultations<ConsultationViewModel>(doctor.Id).Count();

            Assert.Equal(2, consultationsCount);
        }

        [Fact]
        public async Task GettingPatientsConsultationsShouldReturnTheCorrectAmountOfThem()
        {
            var user1 = new ApplicationUser() { UserName = "test", Email = "test@test.com" };
            var user2 = new ApplicationUser() { UserName = "test2", Email = "test2@test.com" };

            var patient = new Patient()
            {
                LastName = "Test",
                UserId = user1.Id,
                User = user1,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            this.doctorsRepository.AddAsync(doctor);
            this.doctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.consultationsService.AddConsultation(model, patient.Id);
            await this.consultationsService.AddConsultation(model, patient.Id);

            var consultationsCount =
                this.consultationsService.GetPatientsConsultations<ConsultationViewModel>(patient.Id).Count();

            Assert.Equal(2, consultationsCount);
        }
    }
}
