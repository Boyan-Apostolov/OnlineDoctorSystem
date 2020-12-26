namespace OnlineDoctorSystem.Web.ViewModels.Prescriptions
{
    using System.Collections.Generic;

    public class ListOfPrescriptions
    {
        public IEnumerable<AddPrescriptionInputModel> Prescriptions { get; set; }
    }
}
