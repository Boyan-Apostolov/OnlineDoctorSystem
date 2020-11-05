using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineDoctorSystem.Services.Data.Towns
{
    public interface ITownsService
    {
        IEnumerable<T> GetAllTowns<T>();
    }
}
