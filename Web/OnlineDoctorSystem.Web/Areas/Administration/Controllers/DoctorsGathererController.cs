using OnlineDoctorSystem.Web.ViewModels.Home;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services;
    using OnlineDoctorSystem.Services.Data.Towns;

    public class DoctorsGathererController : AdministrationController
    {
        private readonly IDoctorScraperService doctorScraperService;
        private readonly ITownsService townsService;

        public DoctorsGathererController(IDoctorScraperService doctorScraperService, ITownsService townsService)
        {
            this.doctorScraperService = doctorScraperService;
            this.townsService = townsService;
        }

        public IActionResult GatherDoctors()
        {
            var model = new IndexViewModel
            {
                Towns = this.townsService.GetAllAsKeyValuePairs(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GatherDoctors(int count, int townId)
        {
            var addedDoctors = await this.doctorScraperService.Import(count, townId);
            return this.RedirectToAction("Added", "DoctorsGatherer", new { addedDoctors = addedDoctors });
        }

        public IActionResult Added(int addedDoctors)
        {
            this.ViewData["Count"] = addedDoctors;
            return this.View();
        }
    }
}
