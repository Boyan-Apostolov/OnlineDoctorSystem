using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Data.Models
{
    public class PatientDoctor
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
