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

        public ConsultationsService(
            IDeletableEntityRepository<Doctor> doctorRepository,
            IDeletableEntityRepository<Consultation> consultationsRepository,
            IDeletableEntityRepository<Patient> patientsRepository,
            IDeletableEntityRepository<CalendarEvent> eventsRepository,
            IEmailSender emailSender, IDoctorsService doctorsService)
        {
            this.doctorRepository = doctorRepository;
            this.consultationsRepository = consultationsRepository;
            this.patientsRepository = patientsRepository;
            this.eventsRepository = eventsRepository;
            this.emailSender = emailSender;
            this.doctorsService = doctorsService;
        }

        public bool CheckIfTimeIsCorrect(AddConsultationViewModel model)
        {
            if (model.StartTime > model.EndTime || model.StartTime == model.EndTime || model.Date <= DateTime.Now)
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

            var patient = await this.patientsRepository.GetByIdWithDeletedAsync(patientId);
            var doctor = await this.doctorRepository.GetByIdWithDeletedAsync(model.DoctorId);

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
            };

            var calendarEvent = new CalendarEvent()
            {
                Color = "yellow",
                Start = consultation.Date + consultation.StartTime,
                End = consultation.Date + consultation.EndTime,
                Text = $"{consultation.StartTime}",
                IsActive = true,
            };

            await this.eventsRepository.AddAsync(calendarEvent);
            await this.eventsRepository.SaveChangesAsync();

            consultation.CalendarEvent = calendarEvent;
            // These are not awaited due to saving problems
            this.consultationsRepository.AddAsync(consultation);
            this.consultationsRepository.SaveChangesAsync();

            doctor.Consultations.Add(consultation);
            this.doctorRepository.SaveChangesAsync();
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
    }
}
