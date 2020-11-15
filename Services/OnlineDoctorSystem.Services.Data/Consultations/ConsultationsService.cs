using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineDoctorSystem.Data.Common.Repositories;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Patients;
using OnlineDoctorSystem.Web.ViewModels.Consultaions;

namespace OnlineDoctorSystem.Services.Data.Consultations
{
    public class ConsultationsService :IConsultationsService
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

        public bool CheckIfTimeIsTaken(AddConsultationViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task AddConsultation(AddConsultationViewModel model,string patientId)
        {
            var doctor = this.doctorRepository.All().FirstOrDefault(x => x.Id == model.DoctorId);
            var consultation = new Consultation()
            {
                Date = model.Date,
                Description = model.Description,
                StartTime = (TimeSpan)model.StartTime,
                EndTime = (TimeSpan)model.EndTime,
                PatientId = patientId,
                DoctorId = doctor.Id,
            };
            this.consultationsRepository.AddAsync(consultation);
            this.consultationsRepository.SaveChangesAsync();
            doctor.Consultations.Add(consultation);
            await this.doctorRepository.SaveChangesAsync();

        }
    }
}
