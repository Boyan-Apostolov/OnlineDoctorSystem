using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    public class DoctorReviewsViewModel
    {
        public string DoctorName { get; set; }

        public ICollection<Data.Models.Review> Reviews { get; set; }
    }
}
