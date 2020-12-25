namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Events;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Data.Prescriptions;
    using OnlineDoctorSystem.Services.Messaging;

    public class BaseTestClass
    {
        public IDoctorsService DoctorsService;
        public IPatientsService PatientsService;
        public IConsultationsService ConsultationsService;
        public IEmailSender EmailSender;
        public IEventsService EventsService;
        public IPrescriptionsService PrescriptionsService;

        public EfDeletableEntityRepository<CalendarEvent> EventsRepository;
        public EfDeletableEntityRepository<Consultation> ConsultationsRepository;
        public EfDeletableEntityRepository<Patient> PatientsRepository;
        public EfDeletableEntityRepository<ApplicationUser> UsersRepository;
        public EfDeletableEntityRepository<Doctor> DoctorsRepository;
        public EfDeletableEntityRepository<Prescription> PrescribtionsRepository;

        public BaseTestClass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.EventsRepository = new EfDeletableEntityRepository<CalendarEvent>(new ApplicationDbContext(options.Options));
            this.ConsultationsRepository = new EfDeletableEntityRepository<Consultation>(new ApplicationDbContext(options.Options));
            this.DoctorsRepository = new EfDeletableEntityRepository<Doctor>(new ApplicationDbContext(options.Options));
            this.UsersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            this.PatientsRepository = new EfDeletableEntityRepository<Patient>(new ApplicationDbContext(options.Options));
            this.PrescribtionsRepository = new EfDeletableEntityRepository<Prescription>(new ApplicationDbContext(options.Options));
            this.EmailSender = new SendGridEmailSender("test");

            this.DoctorsService = new DoctorsService(
                this.DoctorsRepository,
                this.UsersRepository,
                this.EmailSender,
                this.PatientsRepository);

            this.PatientsService = new PatientsService(this.PatientsRepository);

            this.ConsultationsService = new ConsultationsService(
                this.DoctorsRepository,
                this.ConsultationsRepository,
                this.PatientsRepository,
                this.EventsRepository,
                this.EmailSender,
                this.DoctorsService,
                this.PatientsService);

            this.EventsService = new EventsService(
                this.EventsRepository,
                this.ConsultationsRepository,
                this.DoctorsService,
                this.PatientsService,
                this.EmailSender);

            this.PrescriptionsService = new PrescriptionsService(this.PrescribtionsRepository);
        }
    }
}
