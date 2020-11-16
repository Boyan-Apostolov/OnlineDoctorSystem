namespace OnlineDoctorSystem.Services.Data.Consultations
{

    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    public interface IConsultationsService
    {
        bool CheckIfTimeIsCorrect(AddConsultationViewModel model);

        bool AddConsultation(AddConsultationViewModel model, string patientId);
    }
}
