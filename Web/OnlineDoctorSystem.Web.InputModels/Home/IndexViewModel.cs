namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int? TownId { get; set; }

        public int? SpecialtyId { get; set; }

        public string DoctorName { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Towns { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Specialties { get; set; }
    }
}
