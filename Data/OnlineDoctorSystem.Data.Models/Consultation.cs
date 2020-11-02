using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
    public class Consultation : BaseDeletableModel<string>
    {
        public DateTime Date { get; set; }

        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public bool IsActive { get; set; }
    }
}
