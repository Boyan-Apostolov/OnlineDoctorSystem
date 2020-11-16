namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System.Collections.Generic;

    public class AllDoctorViewModel
    {
        public IEnumerable<DoctorViewModelForAll> Doctors { get; set; }
    }
}
