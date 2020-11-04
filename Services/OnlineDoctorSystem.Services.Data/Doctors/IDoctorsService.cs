using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System.Collections.Generic;

    public interface IDoctorsService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        T GetDoctorById<T>(string id);
    }
}
