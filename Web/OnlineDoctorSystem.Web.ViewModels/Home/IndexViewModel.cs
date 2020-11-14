namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class IndexViewModel
    {
        public int? TownId { get; set; }

        public int? SpecialtyId { get; set; }

        public string DoctorName { get; set; }

        public IEnumerable<TownsIndexViewModel> Towns { get; set; }

        public IEnumerable<SpecialtiesIndexViewModel> Specialties { get; set; }
    }
}
