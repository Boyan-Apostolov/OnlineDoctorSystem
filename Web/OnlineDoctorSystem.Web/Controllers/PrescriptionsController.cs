using System.Security.Claims;
using OnlineDoctorSystem.Services.Data.Doctors;
using OnlineDoctorSystem.Services.Data.Patients;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
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
            var viewModel = new PrescriptionViewModel() { PatientId = patientId};
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddPrescriptions(PrescriptionViewModel model)
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
                Prescriptions = this.prescriptionsService.GetPatientsPrescriptions<PrescriptionViewModel>(patient.Id),
            };
            return this.View(viewModel);
        }
    }
}
