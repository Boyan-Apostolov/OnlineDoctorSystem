using System;
using System.Collections.Generic;
using System.Text;
using OnlineDoctorSystem.Data.Common.Models;

namespace OnlineDoctorSystem.Data.Models
{
    public class Specialty : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
