using System.Security.Claims;
using OnlineDoctorSystem.Web.ViewModels.Pateints;

namespace OnlineDoctorSystem.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;

    [Authorize]
    public class PatientsController : Controller
    {
        private readonly IPatientsService patientsService;
        private readonly IDoctorsService doctorsService;

        public PatientsController(
            IPatientsService patientsService,
            IDoctorsService doctorsService)
        {
            this.patientsService = patientsService;
            this.doctorsService = doctorsService;
        }

        [Authorize(Roles = GlobalConstants.PatientRoleName)]
        public IActionResult ThankYou()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        public IActionResult GetDoctorsPatients()
        {
            var doctor = this.doctorsService.GetDoctorByUserId(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var viewModel = new ListAllPatientsViewModel()
            {
                Patients = this.doctorsService.GetDoctorsPatients<PatientForListingViewModel>(doctor.Id),
            };
            return this.View(viewModel);
        }

        public IActionResult GetPatientById(string patientId)
        {
            var viewModel = this.patientsService.GetPatient<PatientViewModel>(patientId);
            return this.View(viewModel);
        }
    }
}
