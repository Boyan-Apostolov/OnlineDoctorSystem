namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Review : BaseDeletableModel<string>
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public double OverallReview { get; set; }

        public double WaitingTimeReview { get; set; }

        public double DoctorAttitudeReview { get; set; }

        public string ReviewText { get; set; }
    }
}
