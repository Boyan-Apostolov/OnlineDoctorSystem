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

        public async Task AddPatientToDb(Patient patient)
        {
            await this.patientRepository.AddAsync(patient);
        }
    }
}
