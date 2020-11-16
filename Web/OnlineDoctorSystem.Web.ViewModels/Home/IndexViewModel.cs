namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int? TownId { get; set; }

        public int? SpecialtyId { get; set; }

        public string DoctorName { get; set; }

        public IEnumerable<TownsIndexViewModel> Towns { get; set; }

        public IEnumerable<SpecialtiesIndexViewModel> Specialties { get; set; }
    }
}
