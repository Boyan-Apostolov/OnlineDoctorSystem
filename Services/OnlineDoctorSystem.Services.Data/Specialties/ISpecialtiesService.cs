namespace OnlineDoctorSystem.Services.Data.Specialties
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface ISpecialtiesService
    {
        IEnumerable<T> GetAllSpecialties<T>();

        Specialty GetSpecialtyById(int id);
    }
}
