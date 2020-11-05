namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Consultation : BaseDeletableModel<string>
    {
        public Consultation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime Date { get; set; }

        public string PatientId { get; set; }

        public Patient Patient { get; set; }

        public string DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public bool IsActive { get; set; }
    }
}
