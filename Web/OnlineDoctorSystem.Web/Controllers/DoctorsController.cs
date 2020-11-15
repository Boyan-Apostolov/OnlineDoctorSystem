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
        private readonly ApplicationDbContext dbContext;
        private readonly ITownsService townsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DoctorsController(
            IDoctorsService doctorsService,
            ApplicationDbContext dbContext,
            ITownsService townsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.doctorsService = doctorsService;
            this.dbContext = dbContext;
            this.townsService = townsService;
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
        public IActionResult AddReview()
        {
            var doctor = this.dbContext.Doctors.First();
            doctor.Reviews.Add(new Review()
            {
                DoctorAttitudeReview = 3.5,
                OverallReview = 4.5,
                WaitingTimeReview = 2.5,
                ReviewText = "Много добре се отнесе с нас, всичко беше много бързо, но той се справи отлично",
            });
            this.dbContext.SaveChanges();
            return this.Content("OK");
        }

        public IActionResult Info(string id)
        {
            var viewModel = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            return this.View(viewModel);
        }

        public IActionResult Reviews(string id)
        {
            var doctor = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            var reviews = doctor.Reviews;
            this.ViewData["DoctorName"] = doctor.Name;
            return this.View(reviews);
        }

        public IActionResult Image(string path)
        {
            return this.PhysicalFile(this.webHostEnvironment.WebRootPath + "/wwwroot" + path, "image/png");
        }
    }
}
