using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Patients
{
    public interface IPatientsService
    {
        Task AddPatientToDb(Patient patient);
    }
}
