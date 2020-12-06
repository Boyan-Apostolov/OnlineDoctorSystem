namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Events;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Messaging;
    using Xunit;

    public class EventsServiceTests
    {
        private readonly IEventsService eventsService;
        private readonly EfDeletableEntityRepository<CalendarEvent> eventsRepository;
        private readonly EfDeletableEntityRepository<Consultation> consultationsRepository;
        private readonly EfDeletableEntityRepository<Patient> patientsRepository;
        private readonly EfDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly EfDeletableEntityRepository<Doctor> doctorsRepository;

        public EventsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.eventsRepository = new EfDeletableEntityRepository<CalendarEvent>(new ApplicationDbContext(options.Options));
            this.consultationsRepository = new EfDeletableEntityRepository<Consultation>(new ApplicationDbContext(options.Options));
            this.doctorsRepository = new EfDeletableEntityRepository<Doctor>(new ApplicationDbContext(options.Options));
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            this.patientsRepository = new EfDeletableEntityRepository<Patient>(new ApplicationDbContext(options.Options));
            var emailSender = new SendGridEmailSender("test");

            var doctorsService = new DoctorsService(
                this.doctorsRepository,
                this.usersRepository,
                emailSender,
                this.consultationsRepository,
                this.patientsRepository);

            var patientsService = new PatientsService(
                this.patientsRepository,
                this.usersRepository);

            this.eventsService = new EventsService(
                this.eventsRepository,
                this.consultationsRepository,
                doctorsService,
                patientsService,
                emailSender);
        }

        [Fact]
        public async Task ChangeEventColorShouldChangeEventsColor()
        {
            var calendarEvent = new CalendarEvent()
            {
                Color = "blue",
            };

            await this.eventsRepository.AddAsync(calendarEvent);
            await this.eventsRepository.SaveChangesAsync();

            this.eventsService.ChangeEventColor(calendarEvent.Id, "pink");

            Assert.True(calendarEvent.Color == "pink");
        }

        [Fact]
        public async Task DeleteEventByIdAsyncShouldRemoveTheEvent()
        {
            var calendarEvent = new CalendarEvent()
            {
                Color = "blue",
            };
            var user = new ApplicationUser()
            {
                Email = "test@test.com",
                EmailConfirmed = true,
            };
            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "test",
                UserId = user.Id,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            await this.consultationsRepository.AddAsync(new Consultation()
            {
                Patient = patient,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
            });
            this.consultationsRepository.SaveChangesAsync();

            bool isDeleted = await this.eventsService.DeleteEventByIdAsync(calendarEvent.Id);

            Assert.True(isDeleted);
        }

        [Fact]
        public async Task MoveEventShouldChangeEventsDate()
        {
            var calendarEvent = new CalendarEvent()
            {
                Color = "blue",
            };
            var user = new ApplicationUser() { Email = "test@test.com", EmailConfirmed = true };
            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "test",
                UserId = user.Id,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            await this.consultationsRepository.AddAsync(new Consultation()
            {
                Patient = patient,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
            });
            this.consultationsRepository.SaveChangesAsync();

            this.eventsService.MoveEvent(
                calendarEvent.Id,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(1));

            Assert.Equal(DateTime.Now.AddDays(1).ToShortDateString(), calendarEvent.End.Date.ToShortDateString());
        }

        [Fact]
        public async Task GetPatientsEventsShouldReturnTheCorrectAmountOfEvents()
        {
            var calendarEvent = new CalendarEvent()
            {
                Color = "blue",
            };
            var user = new ApplicationUser() { Email = "test@test.com", EmailConfirmed = true };
            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "test",
                UserId = user.Id,
            };
            await this.patientsRepository.AddAsync(patient);
            await this.patientsRepository.SaveChangesAsync();

            await this.consultationsRepository.AddAsync(new Consultation()
            {
                Patient = patient,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
                IsConfirmed = true,
            });
            this.consultationsRepository.SaveChangesAsync();

            var eventsCount = this.eventsService.GetPatientsEvents(patient.UserId).Count;

            Assert.Equal(1, eventsCount);
        }

        [Fact]
        public async Task GetDoctorsEventsShouldReturnTheCorrectAmountOfEvents()
        {
            var calendarEvent = new CalendarEvent()
            {
                Color = "blue",
            };
            var user = new ApplicationUser() { Email = "test@test.com", EmailConfirmed = true };
            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                UserId = user.Id,
            };
            await this.doctorsRepository.AddAsync(doctor);
            await this.doctorsRepository.SaveChangesAsync();

            await this.consultationsRepository.AddAsync(new Consultation()
            {
                Doctor = doctor,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
                IsConfirmed = true,
                IsActive = true,
            });
            this.consultationsRepository.SaveChangesAsync();

            var eventsCount = this.eventsService.GetDoctorsEvents(doctor.UserId).Count;

            Assert.Equal(1, eventsCount);
        }
    }
}
