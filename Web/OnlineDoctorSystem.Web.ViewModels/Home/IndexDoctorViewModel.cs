using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    public class IndexDoctorViewModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string AverageRating { get; set; }

        public string Specialty { get; set; }

        public string Town { get; set; }

        public string Url => $"/Doctors/Details/{this.Name.Replace(' ', '-')}";
    }
}
