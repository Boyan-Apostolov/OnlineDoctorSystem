namespace OnlineDoctorSystem.Web.ViewModels.Pateints
{
    using System.Collections.Generic;

    public class ListAllPatientsViewModel
    {
        public IEnumerable<PatientForListingViewModel> Patients { get; set; }
    }
}
