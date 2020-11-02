using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    public class IndexDoctorViewModel : IMapFrom<Doctor>
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string AverageRating { get; set; }

        public string Specialty { get; set; }

        public Town Town { get; set; }

        public string TownName => this.Town.TownName;

        public string Url => $"/Doctors/Details/{this.Name.Replace(' ', '-')}";
    }
}
