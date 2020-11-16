using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    public class ConsultationViewModel : IMapFrom<Consultation>
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
