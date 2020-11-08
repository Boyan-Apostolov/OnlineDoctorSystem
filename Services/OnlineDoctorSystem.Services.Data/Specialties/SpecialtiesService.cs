using System.Threading.Tasks;

namespace OnlineDoctorSystem.Services.Data.Specialties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper.Configuration;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class SpecialtiesService : ISpecialtiesService
    {
        private readonly IDeletableEntityRepository<Specialty> specialtyRepository;

        public SpecialtiesService(IDeletableEntityRepository<Specialty> specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }

        public IEnumerable<T> GetAllSpecialties<T>()
        {
            IQueryable<Specialty> query = this.specialtyRepository.All();
            var specialties = query.To<T>().ToList();
            return specialties;
        }

        public Specialty GetSpecialtyById(int id)
        {
            return this.specialtyRepository.GetByIdWithDeletedAsync(id).Result;
        }
    }
}
