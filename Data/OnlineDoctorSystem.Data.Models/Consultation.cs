namespace OnlineDoctorSystem.Data.Models
{
    using System;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Consultation : BaseDeletableModel<string>
    {
        public Consultation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime { get; set; }

        public DateTime Date { get; set; }

        public string PatientId { get; set; }

        public Patient Patient { get; set; }

        public string DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public bool IsActive { get; set; }

        public bool IsCancelled { get; set; }

        public string Description { get; set; }

        public CalendarEvent CalendarEvent { get; set; }
    }
}
