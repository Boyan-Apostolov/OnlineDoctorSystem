namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Specialty : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
