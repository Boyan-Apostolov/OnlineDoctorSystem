namespace OnlineDoctorSystem.Web.ViewModels.Review
{
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.Infrastructure;

    public class AddReviewInputModel : IMapFrom<Data.Models.Review>
    {
        public double OverallReview { get; set; }

        public double WaitingTimeReview { get; set; }

        public double DoctorAttitudeReview { get; set; }

        public string ReviewText { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
