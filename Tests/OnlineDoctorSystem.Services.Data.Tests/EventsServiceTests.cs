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

    public class EventsServiceTests : BaseTestClass
    {
        [Fact]
        public async Task ChangeEventColorShouldChangeEventsColor()
        {
            var calendarEvent = new CalendarEvent()
            {
                Color = "blue",
            };

            await this.EventsRepository.AddAsync(calendarEvent);
            await this.EventsRepository.SaveChangesAsync();

            await this.EventsService.ChangeEventColor(calendarEvent.Id, "pink");

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
            await this.UsersRepository.AddAsync(user);

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "test",
                UserId = user.Id,
                User = user,
            };
            await this.PatientsRepository.AddAsync(patient);

            await this.ConsultationsRepository.AddAsync(new Consultation()
            {
                Patient = patient,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
            });
            await this.ConsultationsRepository.SaveChangesAsync();

            bool isDeleted = await this.EventsService.DeleteEventByIdAsync(calendarEvent.Id);

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
            await this.UsersRepository.AddAsync(user);

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "test",
                UserId = user.Id,
            };
            await this.PatientsRepository.AddAsync(patient);
            await this.UsersRepository.SaveChangesAsync();

            await this.ConsultationsRepository.AddAsync(new Consultation()
            {
                Patient = patient,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
            });
            await this.ConsultationsRepository.SaveChangesAsync();

            await this.EventsService.MoveEvent(
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
            await this.UsersRepository.AddAsync(user);

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "test",
                UserId = user.Id,
            };
            await this.PatientsRepository.AddAsync(patient);

            await this.ConsultationsRepository.AddAsync(new Consultation()
            {
                Patient = patient,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
                IsConfirmed = true,
                IsActive = true,
            });
            await this.ConsultationsRepository.SaveChangesAsync();

            var eventsCount = this.EventsService.GetPatientsEvents(patient.UserId).Count;

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
            await this.UsersRepository.AddAsync(user);
            await this.UsersRepository.SaveChangesAsync();

            var doctor = new Doctor()
            {
                Name = "Test",
                UserId = user.Id,
            };
            await this.DoctorsRepository.AddAsync(doctor);
            await this.DoctorsRepository.SaveChangesAsync();

            await this.ConsultationsRepository.AddAsync(new Consultation()
            {
                Doctor = doctor,
                Date = DateTime.Now,
                CalendarEvent = calendarEvent,
                IsConfirmed = true,
                IsActive = true,
            });
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.ConsultationsRepository.SaveChangesAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            var eventsCount = this.EventsService.GetDoctorsEvents(doctor.UserId).Count;

            Assert.Equal(1, eventsCount);
        }
    }
}
