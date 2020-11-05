using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Web.ViewModels.Home;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    public class AllDoctorViewModel
    {
        public IEnumerable<DoctorViewModelForAll> Doctors { get; set; }
    }
}
