namespace OnlineDoctorSystem.Services.Data.Towns
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public interface ITownsService
    {
        IEnumerable<TownsIndexViewModel> GetAllTowns();

        Town GetTownById(int id);
    }
}
