using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;

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

        public Patient GetPatientByUserId(string userId)
        {
            return this.patientRepository
                .All().Include(x=>x.Town).FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<string> GetPatientEmailByUserId(string id)
        {
            var patient = this.patientRepository.All()
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);

            return patient.User.Email;
        }

        public string GetPatientEmailByPatientId(string patientId)
        {
            var patient =this.patientRepository.All().Include(x => x.User).FirstOrDefault(x => x.Id == patientId);

            return patient.User.Email;
        }
    }
}