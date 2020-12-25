namespace OnlineDoctorSystem.Web.ViewModels.Pateints
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class PatientForListingViewModel : IMapFrom<Patient>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public string TownName { get; set; }
    }
}
