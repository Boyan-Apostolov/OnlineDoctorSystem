using System;

namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;

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

        public Patient GetPatientByUserEmail(string email)
        {
            return this.patientRepository
                .All().FirstOrDefault(x => x.User.Email == email);
        }
    }
}