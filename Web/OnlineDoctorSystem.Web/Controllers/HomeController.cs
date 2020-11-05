using OnlineDoctorSystem.Services.Data.Specialties;
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
        private readonly ITownsService townsService;
        private readonly ISpecialtiesService specialtiesService;

        public HomeController(ITownsService townsService,ISpecialtiesService specialtiesService)
        {
            this.townsService = townsService;
            this.specialtiesService = specialtiesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                Towns = this.townsService.GetAllTowns<TownsIndexViewModel>(),
                Specialties = this.specialtiesService.GetAllSpecialties<SpecialtiesIndexViewModel>(),
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
