﻿namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services;

    public class DoctorsGathererController : AdministrationController
    {
        private readonly IDoctorScraperService doctorScraperService;

        public DoctorsGathererController(IDoctorScraperService doctorScraperService)
        {
            this.doctorScraperService = doctorScraperService;
        }

        public IActionResult GatherDoctors()
        {
            return this.View();
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
