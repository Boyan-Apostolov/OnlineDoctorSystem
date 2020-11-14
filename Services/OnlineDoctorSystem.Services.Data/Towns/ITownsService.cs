namespace OnlineDoctorSystem.Services.Data.Towns
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OnlineDoctorSystem.Data.Models;

    public interface ITownsService
    {
        IEnumerable<T> GetAllTowns<T>();

        Town GetTownById(int id);
    }
}
