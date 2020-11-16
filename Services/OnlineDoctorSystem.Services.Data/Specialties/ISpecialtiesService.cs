namespace OnlineDoctorSystem.Services.Data.Specialties
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Models;

    public interface ISpecialtiesService
    {
        IEnumerable<T> GetAllSpecialties<T>();

        Specialty GetSpecialtyById(int id);
    }
}
