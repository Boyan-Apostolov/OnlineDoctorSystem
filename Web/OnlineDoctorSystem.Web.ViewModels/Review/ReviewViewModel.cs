using System.ComponentModel.DataAnnotations;
using OnlineDoctorSystem.Web.Infrastructure;

namespace OnlineDoctorSystem.Web.ViewModels.Review
{
    using OnlineDoctorSystem.Services.Mapping;

    public class ReviewViewModel : IMapFrom<Data.Models.Review>
    {
        public double OverallReview { get; set; }

        public double WaitingTimeReview { get; set; }

        public double DoctorAttitudeReview { get; set; }

        public string ReviewText { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
