namespace OnlineDoctorSystem.Services.Data.Towns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using OnlineDoctorSystem.Data.Common.Models;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class TownsService : ITownsService
    {
        private readonly IDeletableEntityRepository<Town> townsRepository;

        public TownsService(IDeletableEntityRepository<Town> townsRepository)
        {
            this.townsRepository = townsRepository;
        }

        public IEnumerable<T> GetAllTowns<T>()
        {
            IQueryable<Town> querry = this.townsRepository.All();
            var towns = querry.To<T>().ToList();
            return towns;
        }

        public Town GetTownById(int id)
        {
            return this.townsRepository.GetByIdWithDeletedAsync(id).Result;
        }
    }
}
