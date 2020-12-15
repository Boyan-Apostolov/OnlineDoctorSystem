namespace OnlineDoctorSystem.Web.ViewModels.Prescriptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ListOfPrescriptions
    {
        public IEnumerable<PrescriptionViewModel> Prescriptions { get; set; }
    }
}
