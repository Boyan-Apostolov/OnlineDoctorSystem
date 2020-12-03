using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Web.ViewModels.Pateints
{
    public class ListAllPatientsViewModel
    {
        public IEnumerable<PatientForListingViewModel> Patients { get; set; }
    }
}
