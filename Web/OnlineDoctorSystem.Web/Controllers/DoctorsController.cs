namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;

    public class DoctorsController : Controller
    {
        private readonly IDoctorsService doctorsService;

        public DoctorsController(IDoctorsService doctorsService)
        {
            this.doctorsService = doctorsService;
        }

        public IActionResult Info(string id)
        {
            var viewModel = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            return this.View(viewModel);
        }
    }
}
