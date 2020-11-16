namespace OnlineDoctorSystem.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

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
