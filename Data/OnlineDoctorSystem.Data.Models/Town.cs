namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
