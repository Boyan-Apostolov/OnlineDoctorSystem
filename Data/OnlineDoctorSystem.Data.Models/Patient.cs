using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Common.Models;
using OnlineDoctorSystem.Data.Models.Enums;

namespace OnlineDoctorSystem.Data.Models
{
    public class Patient : BaseDeletableModel<string>
    {
        public Patient()
        {
            this.Consultations = new HashSet<Consultation>();
            this.Doctors = new HashSet<PatientDoctor>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public Town Town { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; }

        public virtual ICollection<PatientDoctor> Doctors { get; set; }
    }
}
