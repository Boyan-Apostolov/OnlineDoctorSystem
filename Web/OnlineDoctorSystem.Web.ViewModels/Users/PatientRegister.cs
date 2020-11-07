namespace OnlineDoctorSystem.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;

    public class PatientRegister
    {
        [Required]
        [MinLength(3, ErrorMessage = "Името трябва да се състои от минимум 3 символа")]
        [MaxLength(30, ErrorMessage = "Името трябва да се състои от максимум 30 символа")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Фамилията трябва да се състои от минимум 3 символа")]
        [MaxLength(30, ErrorMessage = "Фамилията трябва да се състои от максимум 30 символа")]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public int TownId { get; set; }

        [Required]
        [DataType(DataType.Date)]

        public DateTime BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Паролата трябва да се състои от минимум 6 символа")]
        [MaxLength(25, ErrorMessage = "Паролата трябва да се състои от максимум 25 символа")]
        [PasswordPropertyText]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
