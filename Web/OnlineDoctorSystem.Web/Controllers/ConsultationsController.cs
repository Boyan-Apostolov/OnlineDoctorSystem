using OnlineDoctorSystem.Services.Data.Consultations;
using OnlineDoctorSystem.Services.Data.Patients;
using OnlineDoctorSystem.Web.ViewModels.Consultaions;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;

    public class ConsultationsController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly IConsultationsService consultationsService;
        private readonly IPatientsService patientsService;

        public ConsultationsController(IDoctorsService doctorsService,
            IConsultationsService consultationsService,
            IPatientsService patientsService)
        {
            this.doctorsService = doctorsService;
            this.consultationsService = consultationsService;
            this.patientsService = patientsService;
        }

        public IActionResult AddConsultation(string id)
        {
            var doctor = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            this.TempData["doctorName"] = doctor.Name;
            this.TempData["doctorId"] = doctor.Id;
            return this.View();
        }

        [HttpPost]
        public IActionResult AddConsultation(AddConsultationViewModel model)
        {
            var patientId = this.patientsService.GetPatientIdByEmail(this.User.Identity.Name);
            this.consultationsService.AddConsultation(model, patientId);
            return this.RedirectToAction("SuccessfullyBooked");
        }

        public IActionResult SuccessfullyBooked()
        {
            return this.View();
        }
    }
}
