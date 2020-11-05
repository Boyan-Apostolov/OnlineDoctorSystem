namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PatientDoctor
    {
        public string DoctorId { get; set; }

        public string PatientId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
