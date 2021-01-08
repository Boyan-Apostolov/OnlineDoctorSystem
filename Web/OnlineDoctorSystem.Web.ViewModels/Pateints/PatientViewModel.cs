namespace OnlineDoctorSystem.Web.ViewModels.Pateints
{
    using System;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;
    using OnlineDoctorSystem.Services.Mapping;

    public class PatientViewModel : IMapFrom<Patient>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public string TownName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }
    }
}
