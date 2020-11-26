﻿namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;

    public class PatientsService : IPatientsService
    {
        private readonly IDeletableEntityRepository<Patient> patientRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public PatientsService(
            IDeletableEntityRepository<Patient> patientRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.patientRepository = patientRepository;
            this.usersRepository = usersRepository;
        }

        public async Task AddPatientToDb(string userId, Patient patient)
        {
            patient.UserId = userId;
            await this.patientRepository.AddAsync(patient);
            await this.patientRepository.SaveChangesAsync();
        }

        public string GetPatientIdById(string userId)
        {
            return this.patientRepository.All().FirstOrDefault(x => x.UserId == userId).Id;
        }

        public Patient GetPatientByUserId(string userId)
        {
            var patients = this.patientRepository.All().ToList();
            return this.patientRepository
                .All().FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<string> GetPatientEmailByUserId(string id)
        {
            var patient = await this.patientRepository.GetByIdWithDeletedAsync(id);
            var user = await this.usersRepository.GetByIdWithDeletedAsync(patient.UserId);

            return user.Email;
        }
    }
}