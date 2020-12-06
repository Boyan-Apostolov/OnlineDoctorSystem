using System;
using OnlineDoctorSystem.Common;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Specialties;
    using OnlineDoctorSystem.Services.Data.Towns;
    using OnlineDoctorSystem.Web.ViewModels;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly ITownsService townsService;
        private readonly ISpecialtiesService specialtiesService;

        public HomeController(ITownsService townsService,
            ISpecialtiesService specialtiesService)
        {
            this.townsService = townsService;
            this.specialtiesService = specialtiesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                Towns = this.townsService.GetAllAsKeyValuePairs(),
                Specialties = this.specialtiesService.GetAllAsKeyValuePairs(),
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CustomErrorPage()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpGet("robots.txt")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public IActionResult RobotsTxt() => this.Content("User-agent: *" + Environment.NewLine + "Disallow:");
    }
}
