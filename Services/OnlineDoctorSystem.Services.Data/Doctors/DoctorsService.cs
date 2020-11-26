namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class DoctorsService : IDoctorsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public DoctorsService(IDeletableEntityRepository<Doctor> doctorsRepository
        , IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.doctorsRepository = doctorsRepository;
            this.usersRepository = usersRepository;
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage)
        {
            IQueryable<Doctor> query = this.doctorsRepository.AllAsNoTracking()
                .Where(x => x.User.EmailConfirmed)
                .OrderByDescending(x => x.Consultations.Count)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

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

        public Doctor GetDoctorByUserId(string userId)
        {
            return this.doctorsRepository
                .All().FirstOrDefault(x => x.User.Id == userId);
        }

        public string GetDoctorNameById(string id)
        {
            var doctor = this.doctorsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);
            return doctor.Name;
        }

        public async Task<string> GetDoctorEmailById(string id)
        {
            var doctor = this.doctorsRepository.All().FirstOrDefault(x => x.Id == id);
            var user = await this.usersRepository.GetByIdWithDeletedAsync(doctor.UserId);

            return user.Email;
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

        public int GetDoctorsCount()
        {
            return this.doctorsRepository.AllAsNoTracking().Count();
        }

        public async Task<bool> AddReview(ReviewViewModel model)
        {
            var doctor = this.GetDoctorById(model.DoctorId);

            var review = new Review()
            {
                DoctorAttitudeReview = model.DoctorAttitudeReview,
                OverallReview = model.OverallReview,
                WaitingTimeReview = model.WaitingTimeReview,
                ReviewText = model.ReviewText,
            };

            doctor.Reviews.Add(review);
            await this.doctorsRepository.SaveChangesAsync();
            return true;
        }
    }
}
