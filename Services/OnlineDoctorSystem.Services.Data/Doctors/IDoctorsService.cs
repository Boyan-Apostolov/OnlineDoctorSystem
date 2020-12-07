namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public interface IDoctorsService
    {
        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        IEnumerable<T> GetAllDoctorsNearPatient<T>(int page, int itemsPerPage, Town patientTown);

        T GetDoctorById<T>(string id);

        Doctor GetDoctorById(string id);

        Doctor GetDoctorByUserId(string userId);

        IEnumerable<T> GetDoctorsPatients<T>(string doctorId);

        string GetDoctorNameById(string id);

        Task<string> GetDoctorEmailById(string id);

        Task CreateDoctorAsync(string userId, Doctor doctor);

        Task ApproveDoctorAsync(string doctorId);

        Task DeclineDoctorAsync(string doctorId);

        IEnumerable<T> GetFilteredDoctors<T>(IndexViewModel model);

        IEnumerable<T> GetUnconfirmedDoctors<T>();

        int GetDoctorsCount();

        int GetReviewsCount();

        Task<bool> AddReview(ReviewViewModel model);

    }
}
