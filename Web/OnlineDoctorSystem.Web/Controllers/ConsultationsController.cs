using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Services.Data.Events;

namespace OnlineDoctorSystem.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Web.ViewModels.Consultations;

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

        public IActionResult AddConsultation(string id)
        {
            var doctorName = this.doctorsService.GetDoctorNameById(id);
            var viewModel = new AddConsultationViewModel()
            {
                DoctorId = id,
                DoctorName = doctorName,
                PatientEmail = this.patientsService.GetPatientIdByEmail(this.User.Identity.Name),
            };
            return this.View(viewModel);
        }

        public IActionResult RemoveConsultation(int eventId)
        {
            this.eventsService.DeleteEventByIdAsync(eventId);

            return this.RedirectToAction("GetUsersConsultations");
        }

        [HttpPost]
        public IActionResult AddConsultation(AddConsultationViewModel model)
        {
            var patientId = this.patientsService.GetPatientIdByEmail(this.User.Identity.Name);

            if (this.consultationsService.AddConsultation(model, patientId).Result)
            {
                return this.RedirectToAction("SuccessfullyBooked", model);
            }

            return this.View("InvalidTimeInput");
        }

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
                var patient = this.patientsService.GetPatientByUserEmail(this.User.Identity.Name);
                viewModel.Consultations =
                    this.consultationsService.GetPatientsConsultations<ConsultationViewModel>(patient.Id);
            }
            else if (this.User.IsInRole(GlobalConstants.DoctorRoleName))
            {
                var doctor = this.doctorsService.GetDoctorByUserEmail(this.User.Identity.Name);
                viewModel.Consultations =
                    this.consultationsService.GetDoctorsConsultations<ConsultationViewModel>(doctor.Id);
            }
            return this.View(viewModel);
        }

        public IActionResult UserCalendar()
        {
            // TODO: Make it so it doesnt show all events from db
            return this.View();
        }
    }
}
