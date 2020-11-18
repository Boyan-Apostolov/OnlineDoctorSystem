namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IPatientsService
    {
        Task AddPatientToDb(string userId, Patient patient);

        string GetPatientIdByEmail(string email);

        Patient GetPatientByUserEmail(string email);
    }
}
