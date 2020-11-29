using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Services.Data.Doctors;
using OnlineDoctorSystem.Services.Data.Patients;

namespace OnlineDoctorSystem.Services.Data.Events
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;

    public class EventsService : IEventsService
    {
        private readonly IDeletableEntityRepository<CalendarEvent> eventsRepository;
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;

        public EventsService(
            IDeletableEntityRepository<CalendarEvent> eventsRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository,
            IDoctorsService doctorsService,
            IPatientsService patientsService)
        {
            this.eventsRepository = eventsRepository;
            this.consultationsRepository = consultationsRepository;
            this.doctorsService = doctorsService;
            this.patientsService = patientsService;
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            var @event = this.eventsRepository.All().Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);
            var consultation = this.consultationsRepository.All().Where(x => x.IsActive)
                .FirstOrDefault(x => x.CalendarEvent.Id == id);

            consultation.IsActive = false;
            consultation.IsCancelled = true;
            @event.IsActive = false;

            this.consultationsRepository.SaveChangesAsync();
            await this.eventsRepository.SaveChangesAsync();
            return true;
        }

        public List<CalendarEvent> GetDoctorsEvents(string userId)
        {
            var doctor = this.doctorsService.GetDoctorByUserId(userId);
            var events = this.consultationsRepository.All()
                .Include(x => x.CalendarEvent)
                .Where(x => x.DoctorId == doctor.Id && x.IsActive)
                .Select(x => x.CalendarEvent)
                .ToList();
            return events;
        }

        public List<CalendarEvent> GetPatientsEvents(string userId)
        {
            var patient = this.patientsService.GetPatientByUserId(userId);
            var events = this.consultationsRepository.All()
                .Include(x => x.CalendarEvent)
                .Where(x => x.PatientId == patient.Id && x.IsConfirmed)
                .Select(x => x.CalendarEvent)
                .ToList();
            return events;
        }

        public void MoveEvent(int eventId, DateTime startTime, DateTime endTime)
        {
            var consultation = this.consultationsRepository.All()
                .Include(x => x.CalendarEvent)
                .FirstOrDefault(x => x.CalendarEvent.Id == eventId);

            consultation.Date = startTime.Date;
            consultation.StartTime = startTime.TimeOfDay;
            consultation.EndTime = endTime.TimeOfDay;

            consultation.CalendarEvent.Start = startTime;
            consultation.CalendarEvent.End = endTime;

            this.consultationsRepository.SaveChangesAsync();
            this.eventsRepository.SaveChangesAsync();
        }

        public void ChangeEventColor(int eventId, string color)
        {
            var @event = this.eventsRepository.All().FirstOrDefault(x => x.Id == eventId);
            @event.Color = color;
            this.eventsRepository.SaveChangesAsync();
        }
    }
}
