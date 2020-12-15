namespace OnlineDoctorSystem.Services.Data.Prescriptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

    public class PrescriptionsService : IPrescriptionsService
    {
        private readonly IDeletableEntityRepository<Prescription> prescriptionsRepository;

        public PrescriptionsService(IDeletableEntityRepository<Prescription> prescriptionsRepository)
        {
            this.prescriptionsRepository = prescriptionsRepository;
        }

        public async Task AddPrescriptionToPatient(PrescriptionViewModel model)
        {
            var prescription = new Prescription()
            {
                Doctor = model.Doctor,
                DoctorId = model.Doctor.Id,
                Patient = model.Patient,
                PatientId = model.PatientId,
                Instructions = model.Instructions,
                Quantity = model.Quantity,
                MedicamentName = model.MedicamentName,
            };

            await this.prescriptionsRepository.AddAsync(prescription);
            await this.prescriptionsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetPatientsPrescriptions<T>(string patientId)
        {
            var tm = this.prescriptionsRepository.All().ToList();
            return this.prescriptionsRepository.All()
                .Where(x => x.PatientId == patientId)
                .Include(x => x.Doctor)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();
        }
    }
}
