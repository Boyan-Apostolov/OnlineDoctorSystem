using OnlineDoctorSystem.Services.Data.Prescriptions;

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
    using OnlineDoctorSystem.Services.Messaging;

    public class BaseTestClass
    {
        public IDoctorsService doctorsService;
        public IPatientsService patientsService;
        public IConsultationsService consultationsService;
        public IEmailSender emailSender;
        public IEventsService eventsService;
        public IPrescriptionsService prescriptionsService;

        public EfDeletableEntityRepository<CalendarEvent> eventsRepository;
        public EfDeletableEntityRepository<Consultation> consultationsRepository;
        public EfDeletableEntityRepository<Patient> patientsRepository;
        public EfDeletableEntityRepository<ApplicationUser> usersRepository;
        public EfDeletableEntityRepository<Doctor> doctorsRepository;
        public EfDeletableEntityRepository<Prescription> prescribtionsRepository;

        public BaseTestClass()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.eventsRepository = new EfDeletableEntityRepository<CalendarEvent>(new ApplicationDbContext(options.Options));
            this.consultationsRepository = new EfDeletableEntityRepository<Consultation>(new ApplicationDbContext(options.Options));
            this.doctorsRepository = new EfDeletableEntityRepository<Doctor>(new ApplicationDbContext(options.Options));
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            this.patientsRepository = new EfDeletableEntityRepository<Patient>(new ApplicationDbContext(options.Options));
            this.prescribtionsRepository = new EfDeletableEntityRepository<Prescription>(new ApplicationDbContext(options.Options));
            this.emailSender = new SendGridEmailSender("test");

            this.doctorsService = new DoctorsService(
                this.doctorsRepository,
                this.usersRepository,
                this.emailSender,
                this.consultationsRepository,
                this.patientsRepository);

            this.patientsService = new PatientsService(
                this.patientsRepository,
                this.usersRepository);

            this.consultationsService = new ConsultationsService(
                this.doctorsRepository,
                this.consultationsRepository,
                this.patientsRepository,
                this.eventsRepository,
                this.emailSender,
                this.doctorsService,
                this.patientsService);

            this.eventsService = new EventsService(
                this.eventsRepository,
                this.consultationsRepository,
                this.doctorsService,
                this.patientsService,
                this.emailSender);

            this.prescriptionsService = new PrescriptionsService(prescribtionsRepository);
        }
    }
}
