﻿using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace OnlineDoctorSystem.Web.Controllers
{

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

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

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 12;
            var viewModel = new AllDoctorViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                DoctorsCount = this.doctorsService.GetDoctorsCount(),
                Doctors = this.doctorsService.GetAll<DoctorViewModelForAll>(id, ItemsPerPage),
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

        public IActionResult GetReviews(string id)
        {
            var doctor = this.doctorsService.GetDoctorById<DoctorViewModel>(id);
            var viewModel = new DoctorReviewsViewModel() { DoctorName = doctor.Name, Reviews = doctor.Reviews };
            return this.View(viewModel);
        }

        public IActionResult AddReview(string doctorId)
        {
            var viewModel = new ReviewViewModel()
            {
                DoctorId = doctorId,
                DoctorName = this.doctorsService.GetDoctorNameById(doctorId),

            };
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult AddReview(ReviewViewModel model)
        {
            bool isAdded = this.doctorsService.AddReview(model).Result;
            if (isAdded)
            {
                return this.RedirectToAction("Info", new { id = model.DoctorId });
            }

            return null;
        }
    }
}
