using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OnlineDoctorSystem.Web.ViewModels.Consultaions;

namespace OnlineDoctorSystem.Services.Data.Consultations
{
    public interface IConsultationsService
    {
        bool CheckIfTimeIsTaken(AddConsultationViewModel model);

        Task AddConsultation(AddConsultationViewModel model, string patientId);
    }
}
