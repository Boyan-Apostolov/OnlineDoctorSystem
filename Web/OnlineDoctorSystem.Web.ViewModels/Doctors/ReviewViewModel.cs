namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{

    using OnlineDoctorSystem.Services.Mapping;

    public class ReviewViewModel : IMapTo<Data.Models.Review>
    {
        public string DoctorName { get; set; }

        public string DoctorId { get; set; }

        public double OverallReview { get; set; }

        public double WaitingTimeReview { get; set; }

        public double DoctorAttitudeReview { get; set; }

        public string ReviewText { get; set; }
    }
}
