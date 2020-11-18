using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineDoctorSystem.Services.Data.Consultations
{

    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    public interface IConsultationsService
    {
        bool CheckIfTimeIsCorrect(AddConsultationViewModel model);

        Task<bool> AddConsultation(AddConsultationViewModel model, string patientId);

        IEnumerable<T> GetDoctorsConsultations<T>(string doctorId);

        IEnumerable<T> GetPatientsConsultations<T>(string patientId);
    }
}
