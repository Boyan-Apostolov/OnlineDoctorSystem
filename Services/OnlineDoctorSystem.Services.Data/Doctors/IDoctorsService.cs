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

        T GetDoctorById<T>(string id);

        Doctor GetDoctorById(string id);

        Doctor GetDoctorByUserId(string userId);

        string GetDoctorNameById(string id);

        Task<string> GetDoctorEmailById(string id);

        Task CreateDoctorAsync(string userId, Doctor doctor);

        IEnumerable<T> GetFilteredDoctors<T>(IndexViewModel model);

        int GetDoctorsCount();
        
        Task<bool> AddReview(ReviewViewModel model);
    }
}
