using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OnlineDoctorSystem.Data.Common.Repositories;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Events
{
    public class EventsService : IEventsService
    {
        private readonly IDeletableEntityRepository<CalendarEvent> eventsRepository;
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;

        public EventsService(
            IDeletableEntityRepository<CalendarEvent> eventsRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository)
        {
            this.eventsRepository = eventsRepository;
            this.consultationsRepository = consultationsRepository;
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            var @event = this.eventsRepository.All().Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == id);
            var consultation = this.consultationsRepository.All().Where(x => x.IsActive)
                .FirstOrDefault(x => x.CalendarEvent.Id == id);

            consultation.IsActive = false;
            @event.IsActive = false;

            this.consultationsRepository.SaveChangesAsync();
            this.eventsRepository.SaveChangesAsync();
            return true;
        }
    }
}
