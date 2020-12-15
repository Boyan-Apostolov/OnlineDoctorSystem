using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Web.ViewModels.Prescriptions
{
    public class ListOfPrescriptions
    {
        public IEnumerable<PrescriptionViewModel> Prescriptions { get; set; }
    }
}
