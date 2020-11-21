using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    public class ReviewViewModel : IMapTo<Data.Models.Review>
    {
        public string DoctorId { get; set; }

        public double OverallReview { get; set; }

        public double WaitingTimeReview { get; set; }

        public double DoctorAttitudeReview { get; set; }

        public string ReviewText { get; set; }
    }
}
