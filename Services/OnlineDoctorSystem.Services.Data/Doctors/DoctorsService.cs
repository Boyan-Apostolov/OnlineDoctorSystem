namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class DoctorsService : IDoctorsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;

        public DoctorsService(IDeletableEntityRepository<Doctor> doctorsRepository)
        {
            this.doctorsRepository = doctorsRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Doctor> query = this.doctorsRepository.All();
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetDoctorById<T>(string id)
        {
            return this.doctorsRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public Doctor GetDoctorById(string id)
        {
            return this.doctorsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);
        }

        public Doctor GetDoctorByUserEmail(string email)
        {
            return this.doctorsRepository
                .All().FirstOrDefault(x => x.User.Email == email);
        }

        public string GetDoctorNameById(string id)
        {
            var doctor = this.doctorsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);
            return doctor.Name;
        }

        public async Task CreateDoctorAsync(string userId, Doctor doctor)
        {
            doctor.UserId = userId;
            await this.doctorsRepository.AddAsync(doctor);
            await this.doctorsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetFilteredDoctors<T>(IndexViewModel model)
        {
            IQueryable<Doctor> doctors = this.doctorsRepository.All();

            if (model.DoctorName != null)
            {
                doctors = doctors.Where(x => x.Name.Contains(model.DoctorName));
            }

            if (model.TownId.HasValue)
            {
                doctors = doctors.Where(x => x.Town.Id == model.TownId.Value);
            }

            if (model.SpecialtyId.HasValue)
            {
                doctors = doctors.Where(x => x.Specialty.Id == model.SpecialtyId.Value);
            }

            return doctors.To<T>().ToList();
        }
    }
}
