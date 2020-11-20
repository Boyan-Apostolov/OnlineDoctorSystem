namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public interface IDoctorsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetDoctorById<T>(string id);

        Doctor GetDoctorById(string id);

        Doctor GetDoctorByUserId(string userId);

        string GetDoctorNameById(string id);

        Task CreateDoctorAsync(string userId, Doctor doctor);

        IEnumerable<T> GetFilteredDoctors<T>(IndexViewModel model);

    }
}
