namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System.Collections.Generic;

    public class DoctorReviewsViewModel
    {
        public string DoctorName { get; set; }

        public ICollection<Data.Models.Review> Reviews { get; set; }
    }
}
