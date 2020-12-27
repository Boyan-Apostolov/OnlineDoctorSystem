using Microsoft.AspNetCore.Mvc;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Services;

    public class DoctorsGathererController : AdministrationController
    {
        private readonly IDoctorScraperService doctorScraperService;

        public DoctorsGathererController(IDoctorScraperService doctorScraperService)
        {
            this.doctorScraperService = doctorScraperService;
        }

        public async Task<IActionResult> GatherDoctors()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> GatherDoctors(int count)
        {
            var addedDoctors = await this.doctorScraperService.Import(count);
            return this.RedirectToAction("Added","DoctorsGatherer", new {addedDoctors = addedDoctors});
        }

        public IActionResult Added(int addedDoctors)
        {
            this.ViewData["Count"] = addedDoctors;
            return this.View();
        }
    }
}
