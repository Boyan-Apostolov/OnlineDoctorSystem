using OnlineDoctorSystem.Services.Mapping;

namespace OnlineDoctorSystem.Services.Data.Towns
{
    using System.Collections.Generic;
    using System.Linq;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Home;

    public class TownsService : ITownsService
    {
        private readonly IDeletableEntityRepository<Town> townsRepository;

        public TownsService(IDeletableEntityRepository<Town> townsRepository)
        {
            this.townsRepository = townsRepository;
        }

        public IEnumerable<T> GetAllTowns<T>()
        {
            var towns = this.townsRepository.All()
                .OrderBy(x => x.Name)
                .To<T>()
                .ToList();
            return towns;
        }

        public Town GetTownById(int id)
        {
            return this.townsRepository.GetByIdWithDeletedAsync(id).Result;
        }

        public IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs()
        {
            return this.townsRepository.AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<int, string>(x.Id, x.Name));
        }
    }
}
