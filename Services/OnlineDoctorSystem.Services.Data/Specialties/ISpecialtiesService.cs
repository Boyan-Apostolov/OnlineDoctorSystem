namespace OnlineDoctorSystem.Services.Data.Specialties
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISpecialtiesService
    {
        IEnumerable<T> GetAllSpecialties<T>();
    }
}
