namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class PatientsService : IPatientsService
    {
        private readonly IDeletableEntityRepository<Patient> patientRepository;

        public PatientsService(
            IDeletableEntityRepository<Patient> patientRepository)
        {
            this.patientRepository = patientRepository;
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
                .All().Include(x => x.Town).FirstOrDefault(x => x.UserId == userId);
        }

        public string GetPatientEmailByUserId(string id)
        {
            var patients = this.patientRepository.All().ToList();
            var patient = this.patientRepository.All()
                .Include(x => x.User)
                .FirstOrDefault(x => x.UserId == id);

            return patient.User.Email;
        }

        public string GetPatientEmailByPatientId(string patientId)
        {
            var patient = this.patientRepository.All().Include(x => x.User).FirstOrDefault(x => x.Id == patientId);

            return patient.User.Email;
        }

        public T GetPatient<T>(string patientId)
        {
            var patient = this.patientRepository.AllAsNoTracking()
                .Where(x => x.Id == patientId)
                .To<T>()
                .First();
            return patient;
        }

        public int GetPatientsCount()
        {
            var patients = this.patientRepository.All().ToList();
            return patients.Count();
        }
    }
}
