﻿using System.Collections.Generic;

namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IPatientsService
    {
        Task AddPatientToDb(string userId, Patient patient);

        Patient GetPatientByUserId(string userId);

        Task<string> GetPatientEmailByUserId(string userId);

        string GetPatientEmailByPatientId(string patientId);

        IEnumerable<T> GetDoctorsPatients<T>(string doctorId);

        T GetPatient<T>(string patientId);
    }
}
