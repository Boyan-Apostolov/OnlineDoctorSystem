﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineDoctorSystem.Data.Common.Models;
using OnlineDoctorSystem.Data.Models.Enums;

namespace OnlineDoctorSystem.Data.Models
{
    public class Doctor : BaseDeletableModel<string>
    {
        public Doctor()
        {
            this.Consultations = new HashSet<Consultation>();
            this.Patients = new HashSet<PatientDoctor>();
            this.Reviews = new HashSet<Review>();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DoctorSpecialty Specialty { get; set; }

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

        public string WorkingHours { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public double AverageRating { get; set; }

        public string Education { get; set; }

        public string Qualifications { get; set; }

        public string Biography { get; set; }

        public virtual ICollection<PatientDoctor> Patients { get; set; }

        public ICollection<Consultation> Consultations { get; set; }
    }
}
