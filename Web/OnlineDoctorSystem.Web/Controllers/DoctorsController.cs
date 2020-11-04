using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Web.ViewModels.Review;

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
        private readonly ApplicationDbContext dbContext;

        public DoctorsController(IDoctorsService doctorsService,ApplicationDbContext dbContext)
        {
            this.doctorsService = doctorsService;
            this.dbContext = dbContext;
        }

        public IActionResult Add()
        {
            var doctor = this.dbContext.Doctors.FirstOrDefault(x => x.Id == "a");
            doctor.Reviews.Add(new Review()
            {
                DoctorAttitudeReview = 3.5,
                OverallReview = 4.5,
                WaitingTimeReview = 2.5,
                ReviewText = "Много добре се отнесе с нас, всичко беше много бързо, но той се справи отлично"
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
    }
}
