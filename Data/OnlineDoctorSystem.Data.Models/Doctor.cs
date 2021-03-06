﻿namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Common.Models;
    using OnlineDoctorSystem.Data.Models.Enums;

    public class Doctor : BaseDeletableModel<string>
    {
        public Doctor()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Consultations = new HashSet<Consultation>();
            this.Reviews = new HashSet<Review>();
        }

        public string Name { get; set; }

        public Specialty Specialty { get; set; }

        public Town Town { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public DateTime BirthDate { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Gender Gender { get; set; }

        public double YearsOfPractice { get; set; }

        public bool IsWorkingWithNZOK { get; set; }

        public bool IsWorkingWithChildren { get; set; }

        public string SmallInfo { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public string Education { get; set; }

        public string Qualifications { get; set; }

        public string Biography { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; }

        public bool? IsConfirmed { get; set; }

        public bool IsFromThirdParty { get; set; }

        public string ContactEmailFromThirdParty { get; set; }

        public string LinkFromThirdParty { get; set; }
    }
}
