namespace OnlineDoctorSystem.Web.ViewModels.Doctors
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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

        public string WorkingHours { get; set; }

        public string Education { get; set; }

        public string Qualifications { get; set; }

        public string Biography { get; set; }

        public double AverageRating { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
