namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;

    public class DoctorsService : IDoctorsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;

        public DoctorsService(IDeletableEntityRepository<Doctor> doctorsRepository)
        {
            this.doctorsRepository = doctorsRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Doctor> query = this.doctorsRepository.All();
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetDoctorById<T>(string id)
        {
            var doctor = this.doctorsRepository.All().Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
            return doctor;
        }
    }
}
