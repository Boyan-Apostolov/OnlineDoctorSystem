namespace OnlineDoctorSystem.Services.Data.Towns
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Models;

    public interface ITownsService
    {
        IEnumerable<T> GetAllTowns<T>();

        Town GetTownById(int id);
    }
}
