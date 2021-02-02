namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Data.Towns;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    
    public class DoctorsController : Controller
    {
        private const int ItemsPerPage = 4;

        private readonly IDoctorsService doctorsService;
        private readonly IConsultationsService consultationsService;
        private readonly IPatientsService patientsService;
        private readonly ITownsService townsService;

        public DoctorsController(
            IDoctorsService doctorsService,
            IConsultationsService consultationsService,
            IPatientsService patientsService,
            ITownsService townsService)
        {
            this.doctorsService = doctorsService;
            this.consultationsService = consultationsService;
            this.patientsService = patientsService;
            this.townsService = townsService;
        }

        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        public IActionResult ThankYou()
        {
            return this.View();
        }

        public IActionResult All(int id = 1)
        {
            id = id <= 0 ? 1 : id;
            var viewModel = new AllDoctorViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                DoctorsCount = this.doctorsService.GetDoctorsCount(),
                Doctors = this.doctorsService.GetAll<DoctorViewModelForAll>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult AllNearPatient(int id = 1)
        {
            var patient = this.patientsService.GetPatientByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var doctors =
                this.doctorsService.GetAllDoctorsNearPatient<DoctorViewModelForAll>(id, ItemsPerPage, patient.Town);
            var viewModel = new AllDoctorViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                DoctorsCount = doctors.Count(),
                Doctors = doctors,
                Towns = this.townsService.GetAllTowns<TownWithCoordinatesViewModel>(),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult All(IndexViewModel model)
        {
            var viewModel = new AllDoctorViewModel()
            {
                Doctors = this.doctorsService.GetFilteredDoctors<DoctorViewModelForAll>(model),
            };
            return this.View(viewModel);
        }

        public IActionResult Info(string id)
        {
            var viewModel = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            return this.View(viewModel);
        }

        public IActionResult GetReviews(string id)
        {
            var doctor = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            var viewModel = new DoctorReviewsViewModel() { DoctorName = doctor.Name, Reviews = doctor.Reviews };
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult AddReview(string doctorId, string consultationId)
        {
            var viewModel = new AddReviewInputModel()
            {
                ConsultationId = consultationId,
                DoctorId = doctorId,
                DoctorName = this.doctorsService.GetDoctorNameById(doctorId),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public async Task<IActionResult> AddReview(AddReviewInputModel model)
        {
            if (this.doctorsService.AddReview(model).Result)
            {
                await this.consultationsService.MakeConsultationReviewedToTrue(model.ConsultationId);
                return this.RedirectToAction(nameof(this.Info), new { id = model.DoctorId });
            }

            return this.View(model);
        }

        public async Task<IActionResult> GetUnconfirmedConsultations()
        {
            var doctorId = this.doctorsService.GetDoctorByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).Id;
            var consultations = await this.consultationsService.GetUnconfirmedConsultations(doctorId);
            return this.View(consultations);
        }
    }
}
