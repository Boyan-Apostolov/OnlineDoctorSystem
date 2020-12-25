namespace OnlineDoctorSystem.Web.ViewModels.Prescriptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ListOfPrescriptions
    {
        public IEnumerable<AddPrescriptionInputModel> Prescriptions { get; set; }
    }
}
