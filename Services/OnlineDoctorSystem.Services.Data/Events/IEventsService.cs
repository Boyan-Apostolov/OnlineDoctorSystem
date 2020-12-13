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

        Task MoveEvent(int eventId, DateTime startTime, DateTime endTime);

        Task ChangeEventColor(int eventId, string color);
    }
}
