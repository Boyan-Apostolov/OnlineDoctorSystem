using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OnlineDoctorSystem.Data.Common.Repositories;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Patients
{
    public class PatientsService : IPatientsService
    {
        private readonly IDeletableEntityRepository<Patient> patientRepository;

        public PatientsService(IDeletableEntityRepository<Patient> patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public async Task AddPatientToDb(string userId, Patient patient)
        {
            var patientToAdd = new Patient()
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Phone = patient.Phone,
                ImageUrl = patient.ImageUrl,
                Town = patient.Town,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                UserId = userId,
            };
            await this.patientRepository.AddAsync(patientToAdd);
            await this.patientRepository.SaveChangesAsync();
        }
    }
}
