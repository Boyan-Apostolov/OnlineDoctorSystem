namespace OnlineDoctorSystem.Services.Data.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Messaging;

    public class EventsService : IEventsService
    {
        private readonly IDeletableEntityRepository<CalendarEvent> eventsRepository;
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;
        private readonly IEmailSender emailSender;

        public EventsService(
            IDeletableEntityRepository<CalendarEvent> eventsRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository,
            IDoctorsService doctorsService,
            IPatientsService patientsService,
            IEmailSender emailSender)
        {
            this.eventsRepository = eventsRepository;
            this.consultationsRepository = consultationsRepository;
            this.doctorsService = doctorsService;
            this.patientsService = patientsService;
            this.emailSender = emailSender;
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            var @event = this.eventsRepository.All().Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);
            var consultation = this.consultationsRepository.All().Include(x => x.CalendarEvent).ToList().FirstOrDefault(x => x.CalendarEvent.Id == id);

            consultation.IsActive = false;
            consultation.IsCancelled = true;
            @event.IsActive = false;

            await this.consultationsRepository.SaveChangesAsync();
            await this.eventsRepository.SaveChangesAsync();

            var patientEmail = this.patientsService.GetPatientEmailByPatientId(consultation.PatientId);
            await this.emailSender.SendEmailAsync(
                 GlobalConstants.SystemAdminEmail,
                 "Онлайн-ДокторСистема",
                 patientEmail,
                 "Едно от предстоящите ве събития беше изтрито!",
                 $"Събитието ви за {consultation.Date.ToShortDateString()} от {consultation.StartTime} часа беше изтрито. За повече информация, моля свържете се с практикуващия лекар.");
            return true;
        }

        public List<CalendarEvent> GetDoctorsEvents(string userId)
        {
            var doctor = this.doctorsService.GetDoctorByUserId(userId);
            var events = this.consultationsRepository.All()
                .Include(x => x.CalendarEvent)
                .Where(x => x.DoctorId == doctor.Id && x.IsActive && x.IsConfirmed == true)
                .Select(x => x.CalendarEvent)
                .ToList();
            return events;
        }

        public List<CalendarEvent> GetPatientsEvents(string userId)
        {
            var patient = this.patientsService.GetPatientByUserId(userId);
            var events = this.consultationsRepository.All()
                .Include(x => x.CalendarEvent)
                .Where(x => x.PatientId == patient.Id && x.IsActive && x.IsConfirmed == true)
                .Select(x => x.CalendarEvent)
                .ToList();
            return events;
        }

        public void MoveEvent(int eventId, DateTime startTime, DateTime endTime)
        {
            var consultation = this.consultationsRepository.All()
                .Include(x => x.CalendarEvent)
                .FirstOrDefault(x => x.CalendarEvent.Id == eventId);

            var previousDate = consultation.Date;

            consultation.Date = startTime.Date;
            consultation.StartTime = startTime.TimeOfDay;
            consultation.EndTime = endTime.TimeOfDay;

            consultation.CalendarEvent.Start = startTime;
            consultation.CalendarEvent.End = endTime;

            this.consultationsRepository.SaveChangesAsync();
            this.eventsRepository.SaveChangesAsync();

            var patientEmail = this.patientsService.GetPatientEmailByPatientId(consultation.PatientId);
            this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                patientEmail,
                "Датата на вашата заявка беше променена!",
                $"Вашата консултация от {previousDate.ToShortDateString()} беше преместена на {consultation.Date.ToShortDateString()}.");
        }

        public void ChangeEventColor(int eventId, string color)
        {
            var @event = this.eventsRepository.All().FirstOrDefault(x => x.Id == eventId);
            @event.Color = color;
            this.eventsRepository.SaveChangesAsync();
        }
    }
}
