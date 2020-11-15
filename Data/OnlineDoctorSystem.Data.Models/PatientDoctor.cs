using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PatientDoctor :BaseDeletableModel<int>
    {
        public string DoctorId { get; set; }

        public string PatientId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
