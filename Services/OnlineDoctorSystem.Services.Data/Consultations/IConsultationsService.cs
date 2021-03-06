﻿namespace OnlineDoctorSystem.Services.Data.Consultations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    public interface IConsultationsService
    {
        bool CheckIfTimeIsCorrect(AddConsultationInputModel model);

        Task<bool> AddConsultation(AddConsultationInputModel model, string patientId);

        IEnumerable<T> GetDoctorsConsultations<T>(string doctorId);

        IEnumerable<Consultation> GetDoctorsUnconfirmedConsultations(string doctorId);

        IEnumerable<T> GetPatientsConsultations<T>(string patientId);

        Task ApproveConsultationAsync(string consultationId);

        Task DeclineConsultationAsync(string consultationId);

        Task MakeConsultationReviewedToTrue(string consultationId);

        Task UpdateConsultationsWhenCompleted();

        int GetConsultationsCount();

        Task<IEnumerable<Consultation>> GetUnconfirmedConsultations(string doctorId);
    }
}
