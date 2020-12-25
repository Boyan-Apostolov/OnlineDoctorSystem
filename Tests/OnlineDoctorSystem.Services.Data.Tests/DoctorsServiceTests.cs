namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Consultations;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Home;
    using OnlineDoctorSystem.Web.ViewModels.Pateints;
    using Xunit;

    public class DoctorsServiceTests : BaseTestClass
    {
        public DoctorsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(DoctorViewModel).GetTypeInfo().Assembly);
            AutoMapperConfig.RegisterMappings(typeof(IndexViewModel).GetTypeInfo().Assembly);
            AutoMapperConfig.RegisterMappings(typeof(AddReviewInputModel).GetTypeInfo().Assembly);
            AutoMapperConfig.RegisterMappings(typeof(PatientViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public void GetAllShouldReturnTheCorrectNumberOfEntities()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorsCount = this.DoctorsService.GetAll<DoctorViewModel>(1, 12).Count();
            Assert.Equal(1, doctorsCount);
        }

        [Fact]
        public void GetAllDoctorsNearPatientShouldReturnCorrectAmountOfEntities()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            this.UsersRepository.AddAsync(user);

            var town = new Town() { Name = "TestTown" };
            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
                Town = town,
            };
            this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctors = this.DoctorsService.GetAllDoctorsNearPatient<DoctorViewModel>(1, 12, town);
            var doctorsCount = doctors.Count();
            Assert.IsType<List<DoctorViewModel>>(doctors);
            Assert.Equal(1, doctorsCount);
        }

        [Fact]
        public void GettingDoctorByIdWithModelShouldReturnCorrectDoctor()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorFromService = this.DoctorsService.GetDoctorById<DoctorViewModel>(doctor.Id);

            Assert.Equal(doctor.Id, doctorFromService.Id);
            Assert.Equal(doctor.Name, doctorFromService.Name);
        }

        [Fact]
        public void GettingDoctorByIdShouldReturnCorrectDoctor()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorFromService = this.DoctorsService.GetDoctorById(doctor.Id);

            Assert.Equal(doctor, doctorFromService);
        }

        [Fact]
        public void GettingDoctorByUserIdShouldReturnCorrectDoctor()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorFromService = this.DoctorsService.GetDoctorByUserId(user.Id);

            Assert.Equal(doctor, doctorFromService);
        }

        [Fact]
        public void GettingDoctorNameByIdShouldReturnCorrectName()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorFromService = this.DoctorsService.GetDoctorNameById(doctor.Id);

            Assert.Equal(doctor.Name, doctorFromService);
        }

        [Fact]
        public async Task GettingDoctorEmailByIdShouldReturnCorrectEmail()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var email = await this.DoctorsService.GetDoctorEmailById(doctor.Id);

            Assert.Equal(doctor.User.Email, email);
        }

        [Fact]
        public async Task CreateDoctorAsyncShouldAddDoctorToTheDb()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorsCount = this.DoctorsRepository.All().Count();

            Assert.Equal(1, doctorsCount);
        }

        [Fact]
        public async Task ApprovingDoctorShouldChangeItsIsConfirmedFieldToTrue()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            await this.DoctorsService.ApproveDoctorAsync(doctor.Id);

            Assert.True(doctor.IsConfirmed);
        }

        [Fact]
        public async Task DecliningDoctorShouldChangeItsIsConfirmedFieldToFalse()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            await this.DoctorsService.DeclineDoctorAsync(doctor.Id);

            Assert.False(doctor.IsConfirmed);
        }

        [Fact]
        public async Task GettingFilteredDoctorsByNameShouldReturnTheCorrectDoctors()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                IsConfirmed = true,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);
            var model = new IndexViewModel()
            {
                DoctorName = "Test",
            };

            var filteredDoctors = this.DoctorsService.GetFilteredDoctors<DoctorViewModel>(model).Count();

            Assert.Equal(1, filteredDoctors);
        }

        [Fact]
        public async Task GettingFilteredDoctorsByTownShouldReturnTheCorrectDoctors()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);
            var town = new Town() { Name = "Test" };
            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                Town = town,
                IsConfirmed = true,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);
            var model = new IndexViewModel()
            {
                TownId = town.Id,
            };

            var filteredDoctors = this.DoctorsService.GetFilteredDoctors<DoctorViewModel>(model).Count();

            Assert.Equal(1, filteredDoctors);
        }

        [Fact]
        public async Task GettingFilteredDoctorsBySpecialtyShouldReturnTheCorrectDoctors()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);
            var specialty = new Specialty() { Name = "Test" };
            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
                Specialty = specialty,
                IsConfirmed = true,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);
            var model = new IndexViewModel()
            {
                SpecialtyId = specialty.Id,
            };

            var filteredDoctors = this.DoctorsService.GetFilteredDoctors<DoctorViewModel>(model).Count();

            Assert.Equal(1, filteredDoctors);
        }

        [Fact]
        public async Task GettingUnconfirmedDoctorsShouldReturnTheCorrectAmount()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var unconfirmedDoctors = this.DoctorsService.GetUnconfirmedDoctors<DoctorViewModel>().Count();

            Assert.Equal(1, unconfirmedDoctors);
        }

        [Fact]
        public async Task GettingDoctorsCountShouldReturnTheCorrectAmountOfDoctors()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var doctorsCount = this.DoctorsService.GetDoctorsCount();

            Assert.Equal(1, doctorsCount);
        }

        [Fact]
        public async Task GettingReviewsCountShouldReturnTheCorrectAmountOfReviews()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);

            var reviewsCount = this.DoctorsService.GetReviewsCount();

            Assert.Equal(1, reviewsCount);
        }

        [Fact]
        public async Task AddingSuccessfullyReviewShouldReturnTrue()
        {
            var user = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user,
                UserId = user.Id,
            };
            await this.DoctorsService.CreateDoctorAsync(user.Id, doctor);
            var model = new AddReviewInputModel()
            {
                DoctorId = doctor.Id,
                DoctorAttitudeReview = 3,
                OverallReview = 3,
                WaitingTimeReview = 3,
                ReviewText = "Test",
            };
            var isAdded = await this.DoctorsService.AddReview(model);

            Assert.True(isAdded);
        }

        [Fact]
        public async Task GettingDoctorsPatientsShouldReturnCorrectAmount()
        {
            var user1 = new ApplicationUser() { Email = "test@test.com" };
            var user2 = new ApplicationUser() { Email = "test@test.com" };
            await this.UsersRepository.AddAsync(user1);
            await this.UsersRepository.AddAsync(user2);

            var doctor = new Doctor()
            {
                Name = "Test",
                User = user1,
                UserId = user2.Id,
            };
            var patient = new Patient()
            {
                FirstName = "Test",
                UserId = user2.Id,
                User = user2,
            };
            await this.PatientsRepository.AddAsync(patient);

            doctor.Consultations.Add(new Consultation()
            {
                Date = DateTime.Now.AddDays(5),
                Patient = patient,
                PatientId = patient.Id,
            });
            await this.DoctorsService.CreateDoctorAsync(user1.Id, doctor);

            var patientsCount = this.DoctorsService.GetDoctorsPatients<PatientViewModel>(doctor.Id).Count();

            Assert.Equal(1, patientsCount);
        }
    }
}
