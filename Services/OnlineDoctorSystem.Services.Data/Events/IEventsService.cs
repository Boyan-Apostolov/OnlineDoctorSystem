namespace OnlineDoctorSystem.Services.Data.Events
{
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<bool> DeleteEventByIdAsync(int id);
    }
}
