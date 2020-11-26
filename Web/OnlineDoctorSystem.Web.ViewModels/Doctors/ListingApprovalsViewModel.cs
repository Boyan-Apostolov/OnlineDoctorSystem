namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ListingApprovalsViewModel
    {
        public IEnumerable<ApprovalDoctorViewModel> Doctors { get; set; }
    }
}
