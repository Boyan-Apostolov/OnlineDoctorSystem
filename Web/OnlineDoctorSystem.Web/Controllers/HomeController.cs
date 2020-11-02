using System;
using System.Linq;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Common.Repositories;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Models.Enums;
using OnlineDoctorSystem.Services.Mapping;
using OnlineDoctorSystem.Web.ViewModels;
using OnlineDoctorSystem.Web.ViewModels.Home;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Diagnostics;

    using OnlineDoctorSystem.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;
        private readonly ApplicationDbContext _db;

        public HomeController(IDeletableEntityRepository<Doctor> doctorsRepository,ApplicationDbContext db)
        {
            this.doctorsRepository = doctorsRepository;
            _db = db;
        }

        public IActionResult Index()
        {
            var doctors = this.doctorsRepository.All()
                .To<IndexDoctorViewModel>()
                .ToList();

            var viewModel = new IndexViewModel();
            viewModel.Doctors = doctors;

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
