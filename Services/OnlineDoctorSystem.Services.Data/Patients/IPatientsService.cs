﻿namespace OnlineDoctorSystem.Services.Data.Patients
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public interface IPatientsService
    {
        Task AddPatientToDb(string userId, Patient patient);

        Patient GetPatientByUserId(string userId);

        string GetPatientEmailByUserId(string userId);

        string GetPatientEmailByPatientId(string patientId);

        T GetPatient<T>(string patientId);

        int GetPatientsCount();
    }
}
