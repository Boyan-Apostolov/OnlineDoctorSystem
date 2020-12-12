namespace OnlineDoctorSystem.Web.ViewModels.Consultations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using OnlineDoctorSystem.Web.Infrastructure;

    public class AddConsultationViewModel
    {
        public string DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string PatientId { get; set; }

        [Required(ErrorMessage = "Моля въведете дата на консултацията")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Моля въведете начален час на консултацията")]
        [DataType(DataType.Date)]
        public TimeSpan? StartTime { get; set; }

        [Required(ErrorMessage = "Моля въведете краен час")]
        [DataType(DataType.Date)]
        public TimeSpan? EndTime { get; set; }

        [Required(ErrorMessage = "Моля въведете заявката си.")]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime ReceivedOn => DateTime.UtcNow;

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
