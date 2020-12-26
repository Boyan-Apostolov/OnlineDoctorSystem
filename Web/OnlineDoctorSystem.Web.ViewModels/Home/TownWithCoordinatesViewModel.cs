namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class TownWithCoordinatesViewModel : IMapFrom<Town>
    {
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int DoctorsCount { get; set; }
    }
}
