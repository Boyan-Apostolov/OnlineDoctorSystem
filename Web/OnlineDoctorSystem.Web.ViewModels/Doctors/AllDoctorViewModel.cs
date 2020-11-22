namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Web.ViewModels.Utilities;

    public class AllDoctorViewModel : PagingViewModel
    {
        public IEnumerable<DoctorViewModelForAll> Doctors { get; set; }
    }
}
