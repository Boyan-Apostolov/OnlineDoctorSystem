using System;
using OnlineDoctorSystem.Web.ViewModels.Utilities;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System.Collections.Generic;

    public class AllDoctorViewModel : PagingViewModel
    {
        public IEnumerable<DoctorViewModelForAll> Doctors { get; set; }
    }
}
