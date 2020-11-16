namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Towns;
    using OnlineDoctorSystem.Web.ViewModels.Consultaions;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;
    using OnlineDoctorSystem.Web.ViewModels.Review;
    using OnlineDoctorSystem.Web.ViewModels.Users;

    public class DoctorsController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DoctorsController(
            IDoctorsService doctorsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.doctorsService = doctorsService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }

        public IActionResult All()
        {
            var viewModel = new AllDoctorViewModel()
            {
                Doctors = this.doctorsService.GetAll<DoctorViewModelForAll>(),
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

        public IActionResult Reviews(string id)
        {
            var doctor = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            var viewModel = new DoctorReviewsViewModel() { DoctorName = doctor.Name, Reviews = doctor.Reviews};
            return this.View(viewModel);
        }

        public IActionResult Image(string path)
        {
            return this.PhysicalFile(this.webHostEnvironment.WebRootPath + "/wwwroot" + path, "image/png");
        }
    }
}
