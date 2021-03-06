﻿namespace OnlineDoctorSystem.Web.Hubs
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Web.ViewModels.Chat;

    public class ChatHub : Hub
    {
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;

        public ChatHub(
            IDoctorsService doctorsService,
            IPatientsService patientsService)
        {
            this.doctorsService = doctorsService;
            this.patientsService = patientsService;
        }

        public async Task Send(string message)
        {
            var userId = this.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (this.Context.User.IsInRole(GlobalConstants.DoctorRoleName))
            {
                var doctor = this.doctorsService.GetDoctorByUserId(userId);
                await this.Clients.All.SendAsync(
                    "NewMessage",
                    new Message()
                    {
                        CreatedOn = DateTime.Now.ToShortDateString(),
                        Text = message,
                        ImageUrl = doctor.ImageUrl,
                        User = $"{doctor.Name} (Доктор)",
                        IsDoctor = true,
                        IsAdmin = false,
                    });
            }
            else if (this.Context.User.IsInRole(GlobalConstants.PatientRoleName))
            {
                var patient = this.patientsService.GetPatientByUserId(userId);
                await this.Clients.All.SendAsync(
                    "NewMessage",
                    new Message()
                    {
                        CreatedOn = DateTime.Now.ToShortDateString(),
                        Text = message,
                        ImageUrl = patient.ImageUrl,
                        User = $"{patient.FirstName} {patient.LastName} (Пациент)",
                        IsDoctor = false,
                        IsAdmin = false,
                    });
            }
            else if (this.Context.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                await this.Clients.All.SendAsync(
                    "NewMessage",
                    new Message()
                    {
                        CreatedOn = DateTime.Now.ToShortDateString(),
                        Text = message,
                        ImageUrl = @"https://res.cloudinary.com/du3ohgfpc/image/upload/v1606322301/jrtza0zytvwqeqihpg1m.png",
                        User = "Admin",
                        IsDoctor = false,
                        IsAdmin = true,
                    });
            }
            else
            {
                await this.Clients.All.SendAsync(
                    "NewMessage",
                    new Message()
                    {
                        CreatedOn = DateTime.Now.ToShortDateString(),
                        Text = message,
                        ImageUrl = @"https://res.cloudinary.com/du3ohgfpc/image/upload/v1606322301/jrtza0zytvwqeqihpg1m.png",
                        User = "(From Facebook)",
                        IsDoctor = false,
                        IsAdmin = false,
                    });
            }
        }
    }
}
