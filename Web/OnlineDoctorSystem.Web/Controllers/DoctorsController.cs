namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly IConsultationsService consultationsService;

        public DoctorsController(
            IDoctorsService doctorsService,
            IConsultationsService consultationsService)
        {
            this.doctorsService = doctorsService;
            this.consultationsService = consultationsService;
        }

        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        public IActionResult ThankYou()
        {
            return this.View();
        }

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 4;
            var viewModel = new AllDoctorViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                DoctorsCount = this.doctorsService.GetDoctorsCount(),
                Doctors = this.doctorsService.GetAll<DoctorViewModelForAll>(id, ItemsPerPage),
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
        public IActionResult AddReview(string doctorId)
        {
            var viewModel = new ReviewViewModel()
            {
                DoctorId = doctorId,
                DoctorName = this.doctorsService.GetDoctorNameById(doctorId),

            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult AddReview(ReviewViewModel model)
        {
            if (this.doctorsService.AddReview(model).Result)
            {
                return this.RedirectToAction("Info", new { id = model.DoctorId });
            }

            return this.View(model);
        }

        public async Task<IActionResult> GetUnconfirmedConsultations()
        {
            var doctorId = this.doctorsService.GetDoctorByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).Id;
            var consultations = await this.doctorsService.GetUnconfirmedConsultations(doctorId);
            return this.View(consultations);

        }
    }
}
