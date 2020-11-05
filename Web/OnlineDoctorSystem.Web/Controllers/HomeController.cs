using OnlineDoctorSystem.Services.Data.Towns;

namespace OnlineDoctorSystem.Web.Controllers
{
    using OnlineDoctorSystem.Services.Data.Doctors;

    using System.Diagnostics;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IDoctorsService doctorsService;
        private readonly ITownsService townsService;

        public HomeController(IDoctorsService doctorsService, ITownsService townsService)
        {
            this.doctorsService = doctorsService;
            this.townsService = townsService;
        }

        public IActionResult Index()
        {
            //var viewModel = new IndexViewModel()
            //{
            //    Doctors = this.doctorsService.GetAll<IndexDoctorViewModel>(),
            //};

            var viewModel = new TownsIndexViewModels()
            {
                Towns = this.townsService.GetAllTowns<TownsIndexViewModel>(),
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
