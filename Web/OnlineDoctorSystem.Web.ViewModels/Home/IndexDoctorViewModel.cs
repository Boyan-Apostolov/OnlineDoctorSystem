namespace OnlineDoctorSystem.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class IndexDoctorViewModel : IMapFrom<Doctor>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public double AverageRating { get; set; }

        public string Specialty { get; set; }

        public Town Town { get; set; }

        public string Url { get; set; }

        public bool IsWorkingWithNZOK { get; set; }

        public bool IsWorkingWithChildren { get; set; }
        
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Doctor, IndexDoctorViewModel>()
                .ForMember(
                    x => x.Url,
                    c => c.MapFrom(e => "/Doctors/Info/" + e.Id));
        }
    }
}
