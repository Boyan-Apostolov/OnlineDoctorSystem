namespace OnlineDoctorSystem.Data.Models
{
    using System;

    using OnlineDoctorSystem.Data.Common.Models;

    public class CalendarEvent : BaseDeletableModel<int>
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Text { get; set; }

        public string Color { get; set; }

        public bool IsActive { get; set; }
    }
}
