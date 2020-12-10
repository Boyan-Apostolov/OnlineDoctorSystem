﻿using Microsoft.EntityFrameworkCore;

namespace OnlineDoctorSystem.Services.Data.Consultations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Consultations;

    public class ConsultationsService : IConsultationsService
    {
        private readonly IDeletableEntityRepository<Doctor> doctorRepository;
        private readonly IDeletableEntityRepository<Consultation> consultationsRepository;
        private readonly IDeletableEntityRepository<Patient> patientsRepository;
        private readonly IDeletableEntityRepository<CalendarEvent> eventsRepository;
        private readonly IEmailSender emailSender;
        private readonly IDoctorsService doctorsService;
        private readonly IPatientsService patientsService;

        public ConsultationsService(
            IDeletableEntityRepository<Doctor> doctorRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository,
            IDeletableEntityRepository<Patient> patientsRepository,
            IDeletableEntityRepository<CalendarEvent> eventsRepository,
            IEmailSender emailSender,
            IDoctorsService doctorsService,
            IPatientsService patientsService)
        {
            this.doctorRepository = doctorRepository;
            this.consultationsRepository = consultationsRepository;
            this.patientsRepository = patientsRepository;
            this.eventsRepository = eventsRepository;
            this.emailSender = emailSender;
            this.doctorsService = doctorsService;
            this.patientsService = patientsService;
        }

        public bool CheckIfTimeIsCorrect(AddConsultationViewModel model)
        {
            if (model.StartTime > model.EndTime)
            {
                return false;
            }
            else if (model.StartTime == model.EndTime)
            {
                return false;
            }
            else if (model.Date <= DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddConsultation(AddConsultationViewModel model, string patientId)
        {
            if (!CheckIfTimeIsCorrect(model))
            {
                return false;
            }

            var patient = this.patientsRepository.All().FirstOrDefault(x => x.Id == patientId);
            var doctor = this.doctorRepository.All().FirstOrDefault(x => x.Id == model.DoctorId);

            var consultation = new Consultation()
            {
                Date = model.Date,
                Description = model.Description,
                StartTime = (TimeSpan)model.StartTime,
                EndTime = (TimeSpan)model.EndTime,
                PatientId = patientId,
                DoctorId = doctor.Id,
                IsActive = true,
                IsCancelled = false,
                IsConfirmed = null,
            };

            var calendarEvent = new CalendarEvent()
            {
                Color = "yellow",
                Start = consultation.Date + consultation.StartTime,
                End = consultation.Date + consultation.EndTime,
                Text = $"{consultation.StartTime}",
                IsActive = true,
            };
            consultation.CalendarEvent = calendarEvent;
            doctor.Consultations.Add(consultation);

            await this.consultationsRepository.AddAsync(consultation);
            await this.consultationsRepository.SaveChangesAsync();

            var doctorEmail = await this.doctorsService.GetDoctorEmailById(model.DoctorId);
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"{patient.FirstName} {patient.LastName}",
                doctorEmail,
                "Имате нова заявка за консултация",
                $"Имате нова заявка за консултация от пациент {patient.FirstName} {patient.LastName} за {model.Date.ToShortDateString()} от {model.StartTime} часа. Моля потвърдете или отхвърлете заявката от сайта ни."
                );
            return true;
        }

        public IEnumerable<Consultation> GetDoctorsUnconfirmedConsultations(string doctorId)
        {
            var consultations = this.consultationsRepository.All()
                .Where(x => x.DoctorId == doctorId && !x.IsDeleted);
            return consultations.ToList();
        }

        public IEnumerable<T> GetDoctorsConsultations<T>(string doctorId)
        {
            var consultations = this.consultationsRepository.All()
                .Where(x => x.DoctorId == doctorId && !x.IsDeleted);
            return consultations.To<T>().ToList();
        }

        public IEnumerable<T> GetPatientsConsultations<T>(string patientId)
        {
            var consultations = this.consultationsRepository.All()
                .Where(x => x.PatientId == patientId && !x.IsDeleted);
            return consultations.To<T>().ToList();
        }

        public async Task ApproveConsultationAsync(string consultationId)
        {
            var consultation = await this.consultationsRepository.GetByIdWithDeletedAsync(consultationId);

            consultation.IsConfirmed = true;
            this.consultationsRepository.SaveChangesAsync();

            var patientEmail = this.patientsService.GetPatientEmailByPatientId(consultation.PatientId);
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                patientEmail,
                "Вашата заявка беше одобрена!",
                $"Вашата заявка за консултация на {consultation.Date.ToShortDateString()} от {consultation.StartTime} беше одобрена. Доктора ще ви очаква :)");
        }

        public async Task DeclineConsultationAsync(string consultationId)
        {
            var consultation = this.consultationsRepository.All().FirstOrDefault(x => x.Id == consultationId);

            consultation.IsConfirmed = false;
            this.consultationsRepository.SaveChangesAsync();

            var patientEmail = this.patientsService.GetPatientEmailByPatientId(consultation.PatientId);
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                patientEmail,
                "Вашата заявка беше отхвърлена!",
                $"Вашата заявка за консултация на {consultation.Date.ToShortDateString()} от {consultation.StartTime} беше отхвърлена от доктора ви, за повече информация, моля свържете се с него.");
        }

        public async Task MakeConsultationReviewedToTrue(string consultationId)
        {
            var consultation = this.consultationsRepository.All().FirstOrDefault(x => x.Id == consultationId);

            consultation.IsReviewed = true;

            await this.consultationsRepository.SaveChangesAsync();
        }

        public async Task UpdateConsultationsWhenCompleted()
        {
            var pastConsultations = this.consultationsRepository.AllWithDeleted().Where(x => x.Date <= DateTime.Today || x.EndTime <= DateTime.Now.TimeOfDay).ToList();
            foreach (var consultation in pastConsultations)
            {
                consultation.IsActive = false;
            }

            this.consultationsRepository.SaveChangesAsync();
        }

        public int GetConsultationsCount()
        {
            return this.consultationsRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<Consultation>> GetUnconfirmedConsultations(string doctorId)
        {
            var consultations = this.consultationsRepository.AllAsNoTracking().Where(x => x.IsConfirmed == null && x.DoctorId == doctorId).ToList();
            foreach (var consultation in consultations)
            {
                consultation.Patient = await this.patientsRepository.GetByIdWithDeletedAsync(consultation.PatientId);
            }

            return consultations;
        }
    }
}
