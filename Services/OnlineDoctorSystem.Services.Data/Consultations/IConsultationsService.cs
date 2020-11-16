using System.Collections.Generic;

namespace OnlineDoctorSystem.Services.Data.Consultations
{

    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    public interface IConsultationsService
    {
        bool CheckIfTimeIsCorrect(AddConsultationViewModel model);

        bool AddConsultation(AddConsultationViewModel model, string patientId);

        IEnumerable<T> GetDoctorsConsultations<T>(string doctorId);

        IEnumerable<T> GetPatientsConsultations<T>(string patientId);
    }
}
