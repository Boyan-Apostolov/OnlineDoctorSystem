namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class AllDoctorViewModel
    {
        public IEnumerable<DoctorViewModelForAll> Doctors { get; set; }
    }
}
