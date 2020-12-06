namespace OnlineDoctorSystem.Web.ViewModels.Pateints
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;

    public class ListAllPatientsViewModel
    {
        public IEnumerable<PatientForListingViewModel> Patients { get; set; }
    }
}
