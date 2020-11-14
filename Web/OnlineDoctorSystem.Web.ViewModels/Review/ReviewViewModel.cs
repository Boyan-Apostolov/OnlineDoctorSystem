namespace OnlineDoctorSystem.Web.ViewModels.Review
{
    using AutoMapper;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Settings;

    public class ReviewViewModel : IMapFrom<Data.Models.Review>
    {
        public double OverallReview { get; set; }

        public double WaitingTimeReview { get; set; }

        public double DoctorAttitudeReview { get; set; }

        public string ReviewText { get; set; }

    }
}
