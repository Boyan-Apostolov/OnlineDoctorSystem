namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;

    public interface IPatientsService
    {
        Task AddPatientToDb(string userId, Patient patient);

        string GetPatientIdById(string userId);

        Patient GetPatientByUserId(string Id);

        Task<string> GetPatientEmailByUserId(string userId);
    }
}
