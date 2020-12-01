namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class DoctorsService : IDoctorsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IEmailSender emailSender;
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;
        private readonly IDeletableEntityRepository<Patient> patientsRepository;

        public DoctorsService(
            IDeletableEntityRepository<Doctor> doctorsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IEmailSender emailSender,
            IDeletableEntityRepository<Consultation> consultationsRepository, IDeletableEntityRepository<Patient> patientsRepository)
        {
            this.doctorsRepository = doctorsRepository;
            this.usersRepository = usersRepository;
            this.emailSender = emailSender;
            this.consultationsRepository = consultationsRepository;
            this.patientsRepository = patientsRepository;
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage)
        {
            IQueryable<Doctor> query = this.doctorsRepository.AllAsNoTracking()
                .Where(x => x.IsConfirmed ?? false)
                .OrderByDescending(x => x.Consultations.Count)
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
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                doctorEmail,
                "Вашият профил беше потвърден!",
                $"Вашият профил беше потвърден! Вече сте част от нашето семейство и наши пациенти могат да се свързват с вас!"
            );
        }

        public async Task DeclineDoctorAsync(string doctorId)
        {
            var doctor = this.GetDoctorById(doctorId);

            doctor.IsConfirmed = false;
            await this.doctorsRepository.SaveChangesAsync();

            var doctorEmail = await this.GetDoctorEmailById(doctorId);
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                doctorEmail,
                "Вашата заявка за профил беше отхвърлена!",
                $"Съжеляваме, но профилът Ви не покрива изискванията ни. Можете да виждате другите доктори, но вашият профил няма да може да бъде намерен от пациентите ни!"
            );
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

        public IEnumerable<T> GetUnconfirmedDoctors<T>()
        {
            return this.doctorsRepository.All().Where(x => x.IsConfirmed == null).To<T>().ToList();
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

        public async Task<IEnumerable<Consultation>> GetUnconfirmedConsultations(string doctorId)
        {
            var consultations = this.consultationsRepository.AllAsNoTracking().Where(x => !x.IsConfirmed && x.DoctorId == doctorId).ToList();
            foreach (var consultation in consultations)
            {
                consultation.Patient = await this.patientsRepository.GetByIdWithDeletedAsync(consultation.PatientId);
            }
            return consultations;
        }
    }
}
