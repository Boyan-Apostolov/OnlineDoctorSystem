namespace OnlineDoctorSystem.Services.Data.Events
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;

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
            var @event = this.eventsRepository.All().Where(x => !x.IsDeleted).FirstOrDefault(x => x.Id == id);
            var consultation = this.consultationsRepository.All().Where(x => x.IsActive)
                .FirstOrDefault(x => x.CalendarEvent.Id == id);

            consultation.IsActive = false;
            consultation.IsCancelled = true;
            @event.IsActive = false;

            this.consultationsRepository.SaveChangesAsync();
            await this.eventsRepository.SaveChangesAsync();
            return true;
        }
    }
}
