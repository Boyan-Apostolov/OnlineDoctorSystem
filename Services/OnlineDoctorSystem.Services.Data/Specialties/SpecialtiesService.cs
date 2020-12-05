namespace OnlineDoctorSystem.Services.Data.Specialties
{
    using System.Collections.Generic;
    using System.Linq;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class SpecialtiesService : ISpecialtiesService
    {
        private readonly IDeletableEntityRepository<Specialty> specialtyRepository;

        public SpecialtiesService(IDeletableEntityRepository<Specialty> specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }

        public IEnumerable<SpecialtiesIndexViewModel> GetAllSpecialties()
        {
            return this.specialtyRepository.AllAsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new SpecialtiesIndexViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToList();
        }

        public Specialty GetSpecialtyById(int id)
        {
            return this.specialtyRepository.GetByIdWithDeletedAsync(id).Result;
        }

        public IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs()
        {
            return this.specialtyRepository.AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
        }

        public int GetSpecialtiesCount()
        {
            return this.specialtyRepository.AllAsNoTracking().Count();
        }
    }
}
