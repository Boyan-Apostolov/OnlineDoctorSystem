using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineDoctorSystem.Web.ViewModels.Consultaions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;


    public class AddConsultationViewModel
    {
        public string DoctorId { get; set; }

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

    }
}
