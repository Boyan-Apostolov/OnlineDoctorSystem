using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Services.Data.Consultations
{
    using System;
    using System.Linq;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    public class ConsultationsService : IConsultationsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorRepository;
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;
        private readonly IDeletableEntityRepository<Patient> patientsRepository;
        private readonly IDeletableEntityRepository<CalendarEvent> eventsRepository;

        public ConsultationsService(
            IDeletableEntityRepository<Doctor> doctorRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository,
            IDeletableEntityRepository<Patient> patientsRepository,
            IDeletableEntityRepository<CalendarEvent> eventsRepository)
        {
            this.doctorRepository = doctorRepository;
            this.consultationsRepository = consultationsRepository;
            this.patientsRepository = patientsRepository;
            this.eventsRepository = eventsRepository;
        }

        public bool CheckIfTimeIsCorrect(AddConsultationViewModel model)
        {
            if (model.StartTime > model.EndTime || model.StartTime == model.EndTime || model.Date <= DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddConsultation(AddConsultationViewModel model, string patientId)
        {
            if (!CheckIfTimeIsCorrect(model))
            {
                return false;
            }

            var patient = this.patientsRepository.GetByIdWithDeletedAsync(patientId).Result;
            var doctor = this.doctorRepository.All().FirstOrDefault(x => x.Id == model.DoctorId);

            var consultation = new Consultation()
            {
                Date = model.Date,
                Description = model.Description,
                StartTime = (TimeSpan)model.StartTime,
                EndTime = (TimeSpan)model.EndTime,
                PatientId = patientId,
                DoctorId = doctor.Id,
                IsActive = true,
            };

            var calendarEvent = new CalendarEvent()
            {
                Color = "yellow",
                Start = consultation.Date + consultation.StartTime,
                End = consultation.Date + consultation.EndTime,
                Text = $"{consultation.StartTime}",
            };

            await this.eventsRepository.AddAsync(calendarEvent);
            await this.eventsRepository.SaveChangesAsync();

            consultation.CalendarEvent = calendarEvent;
            // These are not awaited due to saving problems
            this.consultationsRepository.AddAsync(consultation);
            this.consultationsRepository.SaveChangesAsync();

            doctor.Consultations.Add(consultation);
            this.doctorRepository.SaveChangesAsync();
            return true;
        }

        public IEnumerable<T> GetDoctorsConsultations<T>(string doctorId)
        {
            var consultations = this.consultationsRepository.All().Where(x => x.DoctorId == doctorId && x.IsActive);
            return consultations.To<T>().ToList();
        }

        public IEnumerable<T> GetPatientsConsultations<T>(string patientId)
        {
            var consultations = this.consultationsRepository.All().Where(x => x.PatientId == patientId && x.IsActive);
            return consultations.To<T>().ToList();
        }
    }
}
