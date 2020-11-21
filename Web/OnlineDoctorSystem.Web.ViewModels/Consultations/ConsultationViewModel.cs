using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    public class ConsultationViewModel : IMapFrom<Consultation>, IHaveCustomMappings
    {
        public string DoctorName { get; set; }

        public int EventId { get; set; }

        public bool IsActive { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Consultation, ConsultationViewModel>().ForMember(
                    m => m.DoctorName,
                    opt => opt.MapFrom(x => x.Doctor.Name))
                .ForMember(
                    m => m.EventId,
                    opt => opt.MapFrom(x => x.CalendarEvent.Id));
        }
    }
}
