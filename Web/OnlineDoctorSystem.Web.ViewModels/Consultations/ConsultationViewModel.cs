namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    using System;

    using AutoMapper;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class ConsultationViewModel : IMapFrom<Consultation>, IHaveCustomMappings
    {
        public string DoctorName { get; set; }

        public string DoctorId { get; set; }

        public int EventId { get; set; }

        public bool IsActive { get; set; }

        public bool IsCancelled { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Consultation, ConsultationViewModel>().ForMember(
                    m => m.DoctorName,
                    opt => opt.MapFrom(x => x.Doctor.Name))
                .ForMember(m => m.EventId,
                    opt => opt.MapFrom(x => x.CalendarEvent.Id))
                .ForMember(m => m.DoctorId,
                    opt => opt.MapFrom(x => x.DoctorId));
        }
    }
}
