namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class SpecialtiesIndexViewModel : IMapFrom<Specialty>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
