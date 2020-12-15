namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System.Collections.Generic;
    using System.Linq;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class DoctorViewModel : IMapFrom<Doctor>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Specialty Specialty { get; set; }

        public Town Town { get; set; }

        public string ImageUrl { get; set; }

        public string Gender { get; set; }

        public double YearsOfPractice { get; set; }

        public bool IsWorkingWithNZOK { get; set; }

        public bool IsWorkingWithChildren { get; set; }

        public string SmallInfo { get; set; }

        public string Phone { get; set; }

        public string Education { get; set; }

        public string Qualifications { get; set; }

        public string Biography { get; set; }

        public double AverageRating()
        {
            if (this.Reviews.Any())
            {
                return (
                    this.Reviews.Average(x => x.DoctorAttitudeReview) +
                    this.Reviews.Average(x => x.OverallReview) +
                    this.Reviews.Average(x => x.WaitingTimeReview)) / 3;
            }

            return 0;
        }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
