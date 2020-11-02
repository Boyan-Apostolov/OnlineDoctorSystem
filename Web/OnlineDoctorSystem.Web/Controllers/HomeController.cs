using System;
using System.Linq;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Models.Enums;
using OnlineDoctorSystem.Web.ViewModels;
using OnlineDoctorSystem.Web.ViewModels.Home;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Diagnostics;

    using OnlineDoctorSystem.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var doctors = this.dbContext.Doctors
                .Select(x => new IndexDoctorViewModel()
                {
                    Name = x.Name,
                    Town = x.Town.TownName,
                    AverageRating = "4",
                    ImageUrl = x.ImageUrl,
                    Specialty = x.Specialty.ToString(),
                })
                .ToList();
            var viewModel = new IndexViewModel()
            {
                Doctors = doctors
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
