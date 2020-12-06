namespace OnlineDoctorSystem.Services.Data.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IEventsService
    {
        Task<bool> DeleteEventByIdAsync(int id);

        List<CalendarEvent> GetDoctorsEvents(string userId);

        List<CalendarEvent> GetPatientsEvents(string userId);

        void MoveEvent(int eventId, DateTime startTime, DateTime endTime);

        void ChangeEventColor(int eventId, string color);
    }
}
