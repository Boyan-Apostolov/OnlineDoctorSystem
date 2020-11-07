using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Data.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum Gender
    {
        [Display(Name = "Избери пол...")]
        None = 0,
        [Display(Name = "Мъж")]
        Male = 1,
        [Display(Name = "Жена")]
        Female = 2,
    }
}
