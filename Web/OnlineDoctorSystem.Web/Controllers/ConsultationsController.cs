using OnlineDoctorSystem.Services.Messaging;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Events;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    [Authorize]
    public class ConsultationsController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly IConsultationsService consultationsService;
        private readonly IPatientsService patientsService;
        private readonly IEventsService eventsService;

        public ConsultationsController(
            IDoctorsService doctorsService,
            IConsultationsService consultationsService,
            IPatientsService patientsService,
            IEventsService eventsService)
        {
            this.doctorsService = doctorsService;
            this.consultationsService = consultationsService;
            this.patientsService = patientsService;
            this.eventsService = eventsService;
        }

        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult AddConsultation(string id)
        {
            var doctorName = this.doctorsService.GetDoctorNameById(id);
            var temp = this.patientsService.GetPatientByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var viewModel = new AddConsultationViewModel()
            {
                DoctorId = id,
                DoctorName = doctorName,
                PatientId = temp.Id,
            };
            return this.View(viewModel);
        }

        public IActionResult RemoveConsultation(int eventId)
        {
            this.eventsService.DeleteEventByIdAsync(eventId);

            return this.RedirectToAction("GetUsersConsultations");
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult AddConsultation(AddConsultationViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("InvalidTimeInput");
            }

            var patient = this.patientsService.GetPatientByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (this.consultationsService.AddConsultation(model, patient.Id).Result)
            {
                return this.RedirectToAction("SuccessfullyBooked", model);
            }

            return this.View("InvalidTimeInput");
        }

        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult SuccessfullyBooked(AddConsultationViewModel model)
        {
            var doctorName = this.doctorsService.GetDoctorNameById(model.DoctorId);
            var viewModel = new SuccessfullyBookedViewModel() { Date = model.Date, DoctorName = doctorName };
            return this.View(viewModel);
        }

        public IActionResult GetUsersConsultations()
        {
            var viewModel = new AllConsultationsViewModel();
            if (this.User.IsInRole(GlobalConstants.PatientRoleName))
            {
                var patient = this.patientsService.GetPatientByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                viewModel.Consultations =
                    this.consultationsService.GetPatientsConsultations<ConsultationViewModel>(patient.Id);
            }
            else if (this.User.IsInRole(GlobalConstants.DoctorRoleName))
            {
                var doctor = this.doctorsService.GetDoctorByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                viewModel.Consultations =
                    this.consultationsService.GetDoctorsConsultations<ConsultationViewModel>(doctor.Id);
            }
            return this.View(viewModel);
        }

        public IActionResult UserCalendar()
        {
            return this.View();
        }
    }
}
