namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System.Collections.Generic;

    public class ListingApprovalsViewModel
    {
        public IEnumerable<ApprovalDoctorViewModel> Doctors { get; set; }
    }
}
