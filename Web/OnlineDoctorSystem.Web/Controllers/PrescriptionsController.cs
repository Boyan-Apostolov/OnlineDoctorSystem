namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Data.Prescriptions;
    using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

    [Authorize]
    public class PrescriptionsController : Controller
    {
        private readonly IPrescriptionsService prescriptionsService;
        private readonly IPatientsService patientsService;
        private readonly IDoctorsService doctorsService;

        public PrescriptionsController(
            IPrescriptionsService prescriptionsService,
            IPatientsService patientsService,
            IDoctorsService doctorsService)
        {
            this.prescriptionsService = prescriptionsService;
            this.patientsService = patientsService;
            this.doctorsService = doctorsService;
        }

        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        public IActionResult AddPrescriptions(string patientId)
        {
            var viewModel = new AddPrescriptionInputModel()
            {
                PatientId = patientId,
            };
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddPrescriptions(AddPrescriptionInputModel model)
        {
            var doctor = this.doctorsService.GetDoctorByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.Doctor = doctor;
            await this.prescriptionsService.AddPrescriptionToPatient(model);
            return this.RedirectToAction("GetDoctorsPatients", "Patients");
        }

        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult GetPatientsPrescriptions()
        {
            var patient =
                this.patientsService.GetPatientByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var viewModel = new ListOfPrescriptions()
            {
                Prescriptions = this.prescriptionsService.GetPatientsPrescriptions<AddPrescriptionInputModel>(patient.Id),
            };
            return this.View(viewModel);
        }
    }
}
