using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Services.Data.Specialties
{
    public interface ISpecialtiesService
    {
        IEnumerable<T> GetAllSpecialties<T>();
    }
}
