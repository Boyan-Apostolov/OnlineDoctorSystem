namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IPatientsService
    {
        Task AddPatientToDb(Patient patient);
    }
}
