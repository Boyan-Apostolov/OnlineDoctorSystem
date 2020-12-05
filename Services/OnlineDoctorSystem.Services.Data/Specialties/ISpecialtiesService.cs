namespace OnlineDoctorSystem.Services.Data.Specialties
{
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public interface ISpecialtiesService
    {
        IEnumerable<SpecialtiesIndexViewModel> GetAllSpecialties();

        Specialty GetSpecialtyById(int id);

        IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs();
        
        int GetSpecialtiesCount();
    }
}
