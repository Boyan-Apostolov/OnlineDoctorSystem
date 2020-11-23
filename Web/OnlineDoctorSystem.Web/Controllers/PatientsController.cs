namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;

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
    }
}
