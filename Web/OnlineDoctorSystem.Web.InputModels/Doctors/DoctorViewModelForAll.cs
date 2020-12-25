namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using AutoMapper;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class DoctorViewModelForAll : IMapFrom<Doctor>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public double AverageRating { get; set; }

        public Specialty Specialty { get; set; }

        public Town Town { get; set; }

        public string Url { get; set; }

        public bool IsWorkingWithNZOK { get; set; }

        public bool IsWorkingWithChildren { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Doctor, DoctorViewModelForAll>()
                .ForMember(
                    x => x.Url,
                    c => c.MapFrom(e => "/Doctors/Info/" + e.Id));
        }
    }
}
