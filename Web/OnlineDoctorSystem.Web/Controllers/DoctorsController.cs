using OnlineDoctorSystem.Services.Data.Towns;
using OnlineDoctorSystem.Web.ViewModels.Users;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Review;

    public class DoctorsController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly ApplicationDbContext dbContext;
        private readonly ITownsService townsService;

        public DoctorsController(IDoctorsService doctorsService, ApplicationDbContext dbContext, ITownsService townsService)
        {
            this.doctorsService = doctorsService;
            this.dbContext = dbContext;
            this.townsService = townsService;
        }

        public IActionResult Alls()
        {
            return this.View();
        } 

        [HttpPost]
        public IActionResult Alls(PatientRegister model)
        {
            return null;
        }

        public IActionResult All()
        {
            var viewModel = new AllDoctorViewModel()
            {
                Doctors = this.doctorsService.GetAll<DoctorViewModelForAll>(),
            };
            return this.View(viewModel);
        }

        public IActionResult AddDoctor()
        {
            this.dbContext.Doctors.AddAsync(new Doctor()
            {
                Name = "Иван Георгиев",
                Town = this.dbContext.Towns.Skip(2).First(),
                Email = "ivan@abv.bg",
                Specialty = this.dbContext.Specialties.Skip(3).First(),
                Phone = "0998765432",
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                YearsOfPractice = 3,
                ImageUrl = "https://homewoodfamilyaz.org/wp-content/uploads/2015/07/male-doctor-3.png",
            });
            this.dbContext.Doctors.AddAsync(new Doctor()
            {
                Name = "Милена Иванова",
                Town = this.dbContext.Towns.Skip(3).First(),
                Email = "mimi@abv.bg",
                Specialty = this.dbContext.Specialties.Skip(5).First(),
                Phone = "09942354642",
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Female,
                YearsOfPractice = 3,
                IsWorkingWithChildren = true,
                IsWorkingWithNZOK = true,
                ImageUrl = "https://thumbs.dreamstime.com/b/beautiful-young-female-doctor-9182291.jpg",
            });
            this.dbContext.SaveChanges();
            return this.Content("OK");
        }

        public IActionResult AddReview()
        {
            var doctor = this.dbContext.Doctors.Skip(1).First();
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
    }
}
