// ReSharper disable All
namespace OnlineDoctorSystem.Services.Data.Tests
{
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
    using OnlineDoctorSystem.Web.ViewModels.Pateints;
    using Xunit;

    public class ConsultationsServiceTests : BaseTestClass
    {
        public ConsultationsServiceTests()
        {
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

            var isTimeCorrect = this.ConsultationsService.CheckIfTimeIsCorrect(model);

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

            var isTimeCorrect = this.ConsultationsService.CheckIfTimeIsCorrect(model);

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

            var isTimeCorrect = this.ConsultationsService.CheckIfTimeIsCorrect(model);

            Assert.False(isTimeCorrect);
        }

        [Fact]
        public void IncorrectTimeShouldReturnFalseWhenEndTimeIsBeforeStartTime()
        {
            var model = new AddConsultationViewModel()
            {
                StartTime = DateTime.Now.AddHours(3).TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay,
                Date = DateTime.Now.AddDays(2),
            };

            var isAdded = this.ConsultationsService.CheckIfTimeIsCorrect(model);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = TimeSpan.Parse("15:15:15"),
                EndTime = TimeSpan.Parse("15:15:15"),
                Date = DateTime.Now.AddDays(1),
            };

            var isAdded = await this.ConsultationsService.AddConsultation(model, patient.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.AddMinutes(40).TimeOfDay,
                Date = DateTime.Now.AddDays(-1),
            };

            var isAdded = await this.ConsultationsService.AddConsultation(model, patient.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddDays(5).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(2),
            };

            var isTimeCorrect = this.ConsultationsService.CheckIfTimeIsCorrect(model);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.Now.AddHours(5).TimeOfDay,
                EndTime = DateTime.Now.AddMinutes(40).TimeOfDay,
                Date = DateTime.Now.AddDays(-1),
            };

            var isAdded = await this.ConsultationsService.AddConsultation(model, patient.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };

            var isAdded = await this.ConsultationsService.AddConsultation(model, patient.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultationsCount = this.ConsultationsService.GetConsultationsCount();

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.ConsultationsRepository.All().FirstAsync();
            consultation.Date = DateTime.Today.AddDays(-1);
            await this.ConsultationsRepository.SaveChangesAsync();

            await this.ConsultationsService.UpdateConsultationsWhenCompleted();

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.ConsultationsRepository.All().FirstAsync();

            await this.ConsultationsService.MakeConsultationReviewedToTrue(consultation.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.ConsultationsRepository.All().FirstAsync();

            await this.ConsultationsService.DeclineConsultationAsync(consultation.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultation = await this.ConsultationsRepository.All().FirstAsync();

            await this.ConsultationsService.ApproveConsultationAsync(consultation.Id);

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var unconfirmedConsultations =
                this.ConsultationsService.GetDoctorsUnconfirmedConsultations(doctor.Id).Count();

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultationsCount =
                this.ConsultationsService.GetDoctorsConsultations<ConsultationViewModel>(doctor.Id).Count();

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
            await this.PatientsRepository.AddAsync(patient);
            await this.PatientsRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user2,
                UserId = user2.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            var model = new AddConsultationViewModel()
            {
                DoctorId = doctor.Id,
                StartTime = DateTime.UtcNow.TimeOfDay,
                EndTime = DateTime.UtcNow.AddMinutes(40).TimeOfDay,
                Date = DateTime.UtcNow.AddDays(5),
            };
            await this.ConsultationsService.AddConsultation(model, patient.Id);
            await this.ConsultationsService.AddConsultation(model, patient.Id);

            var consultationsCount =
                this.ConsultationsService.GetPatientsConsultations<ConsultationViewModel>(patient.Id).Count();

            Assert.Equal(2, consultationsCount);
        }

        [Fact]
        public async Task GettingUnconfirmedConsultationsShouldReturnTheCorrectAmount()
        {
            var user1 = new ApplicationUser() { Email = "test@test.com" };
            var user2 = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user1);
            await this.UsersRepository.AddAsync(user2);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user1,
                UserId = user2.Id,
            };
            var patient = new Patient()
            {
                FirstName = "Test",
                UserId = user2.Id,
                User = user2,
            };
            await this.PatientsRepository.AddAsync(patient);

            doctor.Consultations.Add(new Consultation()
            {
                Date = DateTime.Now.AddDays(5),
                Patient = patient,
                PatientId = patient.Id,
            });
            await this.DoctorsService.CreateDoctorAsync(user1.Id, doctor);

            var consultations = await this.ConsultationsService.GetUnconfirmedConsultations(doctor.Id);
            var consultationsCount = consultations.Count();

            Assert.Equal(1, consultationsCount);
        }
    }
}
