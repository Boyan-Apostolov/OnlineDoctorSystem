using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.Emails;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class DoctorsService : IDoctorsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IEmailsService emailsService;
        private readonly IDeletableEntityRepository<Patient> patientsRepository;

        public DoctorsService(
            IDeletableEntityRepository<Doctor> doctorsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IEmailsService emailsService,
            IDeletableEntityRepository<Patient> patientsRepository)
        {
            this.doctorsRepository = doctorsRepository;
            this.usersRepository = usersRepository;
            this.emailsService = emailsService;
            this.patientsRepository = patientsRepository;
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage)
        {
            IQueryable<Doctor> query = this.doctorsRepository.AllAsNoTracking()
                .Where(x => x.IsConfirmed ?? false)
                .OrderByDescending(x => x.Consultations.Count)
                .ThenBy(x => x.IsFromThirdParty)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage);

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllDoctorsNearPatient<T>(int page, int itemsPerPage, Town patientTown)
        {
            IQueryable<Doctor> query = this.doctorsRepository.AllAsNoTracking()
                .Where(x => x.IsConfirmed == true && x.Town == patientTown)
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

        public async Task ApproveDoctorAsync(string doctorId)
        {
            var doctor = this.GetDoctorById(doctorId);

            doctor.IsConfirmed = true;
            await this.doctorsRepository.SaveChangesAsync();

            var doctorEmail = await this.GetDoctorEmailById(doctorId);

            await this.emailsService.ApproveDoctorEmailAsync(doctorEmail);
        }

        public async Task DeclineDoctorAsync(string doctorId)
        {
            var doctor = this.GetDoctorById(doctorId);

            doctor.IsConfirmed = false;
            await this.doctorsRepository.SaveChangesAsync();

            var doctorEmail = await this.GetDoctorEmailById(doctorId);

            await this.emailsService.DeclineDoctorEmailAsync(doctorEmail);
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

            return doctors.Where(x => x.IsConfirmed == true).To<T>().ToList();
        }

        public IEnumerable<T> GetUnconfirmedDoctors<T>()
        {
            return this.doctorsRepository.All().Where(x => x.IsConfirmed == null).To<T>().ToList();
        }

        public int GetDoctorsCount()
        {
            return this.doctorsRepository.AllAsNoTracking().Count();
        }

        public int GetReviewsCount()
        {
            return this.doctorsRepository.All().Include(x => x.Reviews).ToList().Sum(x => x.Reviews.Count);
        }

        public async Task<bool> AddReview(AddReviewInputModel model)
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

        public IEnumerable<T> GetDoctorsPatients<T>(string doctorId)
        {
            var patients = this.patientsRepository.AllAsNoTracking()
                .Include(x => x.Consultations)
                .Where(x => x.Consultations.Any(x => x.DoctorId == doctorId))
                .To<T>()
                .ToList();

            return patients;
        }

        public async Task DoctorSendEmail(DoctorEmailViewModel model)
        {
            await this.emailsService.DoctorToPatientEmail(model);
        }
    }
}
