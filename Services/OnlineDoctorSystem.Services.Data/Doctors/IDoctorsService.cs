using System.Threading.Tasks;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using OnlineDoctorSystem.Web.ViewModels.Home;
    using System.Collections.Generic;

    public interface IDoctorsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetDoctorById<T>(string id);

        Task AddDoctorToDb(Doctor doctor);

        IEnumerable<T> GetFilteredDoctors<T>(IndexViewModel model);
    }
}
