namespace OnlineDoctorSystem.Services.Data.Prescriptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Web.ViewModels.Prescriptions;

    public interface IPrescriptionsService
    {
        Task AddPrescriptionToPatient(AddPrescriptionInputModel model);

        IEnumerable<T> GetPatientsPrescriptions<T>(string patientId);
    }
}
