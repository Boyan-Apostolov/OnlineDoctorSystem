﻿using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
    public class Prescription : BaseDeletableModel<string>
    {
        public Prescription()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        public string PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public string Quantity { get; set; }

        public string MedicamentName { get; set; }

        public string Instructions { get; set; }
    }
}
