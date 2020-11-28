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

        public IEnumerable<TownsIndexViewModel> GetAllTowns()
        {
            IQueryable<Town> querry = this.townsRepository.All();
            var towns = querry
                .OrderBy(x => x.Name)
                .Select(x => new TownsIndexViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToList();
            return towns;
        }

        public Town GetTownById(int id)
        {
            return this.townsRepository.GetByIdWithDeletedAsync(id).Result;
        }
    }
}
