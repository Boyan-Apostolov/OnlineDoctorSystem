using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<IndexDoctorViewModel> Doctors { get; set; }
    }
}
