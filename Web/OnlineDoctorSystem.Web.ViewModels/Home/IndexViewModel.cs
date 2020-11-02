using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        private IEnumerable<IndexDoctorViewModel> Doctors { get; set; }
    }
}
