using System.Collections.Generic;
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

        public ConsultationsService(
            IDeletableEntityRepository<Doctor> doctorRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository,
            IDeletableEntityRepository<Patient> patientsRepository)
        {
            this.doctorRepository = doctorRepository;
            this.consultationsRepository = consultationsRepository;
        }

        public bool CheckIfTimeIsCorrect(AddConsultationViewModel model)
        {
            if (model.StartTime > model.EndTime || model.StartTime == model.EndTime || model.Date <= DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public bool AddConsultation(AddConsultationViewModel model, string patientId)
        {
            if (!CheckIfTimeIsCorrect(model))
            {
                return false;
            }

            var doctor = this.doctorRepository.All().FirstOrDefault(x => x.Id == model.DoctorId);
            var consultation = new Consultation()
            {
                Date = model.Date,
                Description = model.Description,
                StartTime = (TimeSpan)model.StartTime,
                EndTime = (TimeSpan)model.EndTime,
                PatientId = patientId,
                DoctorId = doctor.Id,
                IsActive = true
            };

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
