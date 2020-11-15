using System;
using System.Collections.Generic;
using System.Linq;
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
            patient.UserId = userId;
            await this.patientRepository.AddAsync(patient);
            await this.patientRepository.SaveChangesAsync();
        }

        public string GetPatientIdByEmail(string email)
        {
            return this.patientRepository.All().FirstOrDefault(x => x.User.Email == email).Id;
        }
    }
}
