namespace OnlineDoctorSystem.Services.Data.Doctors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Home;

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
            var doctor = this.doctorsRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
            return doctor;
        }

        public Doctor GetDoctorById(string id)
        {
            var doctor = this.doctorsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);
            return doctor;
        }

        public void AddConsultation(Consultation consultation)
        {
            var doctor = this.doctorsRepository.All().FirstOrDefault(x => x.Id == consultation.DoctorId);
            doctor.Consultations.Add(consultation);
            this.doctorsRepository.SaveChangesAsync();
        }

        public async Task AddDoctorToDb(Doctor doctor)
        {
            await this.doctorsRepository.AddAsync(doctor);
        }

        public IEnumerable<T> GetFilteredDoctors<T>(IndexViewModel model)
        {
            IQueryable<Doctor> doctors = this.doctorsRepository.All();

            if (model.DoctorName != null)
            {
                doctors = doctors.Where(x => x.Name.Contains(model.DoctorName));
            }

            if (model.TownId.HasValue)
            {
                doctors = doctors.Where(x => x.Town.Id == model.TownId.Value);
            }

            if (model.SpecialtyId.HasValue)
            {
                doctors = doctors.Where(x => x.Specialty.Id == model.SpecialtyId.Value);
            }

            return doctors.To<T>().ToList();
        }
    }
}
