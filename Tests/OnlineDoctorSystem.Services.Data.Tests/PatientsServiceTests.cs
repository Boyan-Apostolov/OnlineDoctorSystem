namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Moq;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.ContactSubmission;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Data.Specialties;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;
    using OnlineDoctorSystem.Web.ViewModels.Pateints;
    using Xunit;

    public class PatientsServiceTests
    {
        private readonly IList<Patient> list;
        private readonly IList<ApplicationUser> listOfUsers;
        private readonly IPatientsService patientsService;

        public PatientsServiceTests()
        {
            this.list = new List<Patient>();
            this.listOfUsers = new List<ApplicationUser>();

            var mockRepo = new Mock<IDeletableEntityRepository<Patient>>();
            var mockRepoOfUser = new Mock<IDeletableEntityRepository<ApplicationUser>>();

            mockRepo.Setup(x => x.All()).Returns(this.list.AsQueryable());
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(this.list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Patient>())).Callback((Patient patient) => this.list.Add(patient));

            mockRepoOfUser.Setup(x => x.All()).Returns(this.listOfUsers.AsQueryable());
            mockRepoOfUser.Setup(x => x.AddAsync(It.IsAny<ApplicationUser>())).Callback((ApplicationUser user) => this.listOfUsers.Add(user));

            AutoMapperConfig.RegisterMappings(
                typeof(PatientViewModel).GetTypeInfo().Assembly);

            var service = new PatientsService(mockRepo.Object);
            this.patientsService = service;
        }

        [Fact]
        public async Task SuccessfullyAddPatientToDb()
        {
            var userId = "sample-user-id";
            var patient = new Patient()
            {
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                ImageUrl = "test",
                Town = new Town() { Name = "test" },
                BirthDate = DateTime.UtcNow,
                UserId = userId,
            };

            await this.patientsService.AddPatientToDb(userId, patient);

            var createdPatient = this.list.FirstOrDefault(x => x.UserId == userId);

            Assert.Equal(patient, createdPatient);
        }

        [Fact]
        public async Task GetPatientByIdShouldWork()
        {
            var userId = "sample-user-id";
            var patient = new Patient()
            {
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                ImageUrl = "test",
                Town = new Town() { Name = "test" },
                BirthDate = DateTime.UtcNow,
                UserId = userId,
            };

            await this.patientsService.AddPatientToDb(userId, patient);

            var patientById = this.patientsService.GetPatientByUserId(userId);

            Assert.Equal(patient, patientById);
        }

        [Fact]
        public void GetPatientEmailByUserIdShouldGetCorrectEmail()
        {
            var email = "test@test.com";
            var user = new ApplicationUser() { UserName = "test", Email = email };

            var patient = new Patient()
            {
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                ImageUrl = "test",
                Town = new Town() { Name = "test" },
                BirthDate = DateTime.UtcNow,
                UserId = user.Id,
                User = user,
            };

            this.listOfUsers.Add(user);
            this.list.Add(patient);

            var actualEmail = this.patientsService.GetPatientEmailByUserId(user.Id);

            Assert.Equal(email, actualEmail);
        }

        [Fact]
        public async Task GetPatientsCountShouldReturnTheCorrectAmount()
        {
            var patient = new Patient()
            {
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                ImageUrl = "test",
                Town = new Town() { Name = "test" },
                BirthDate = DateTime.UtcNow,
            };

            await this.patientsService.AddPatientToDb("test", patient);
            await this.patientsService.AddPatientToDb("test2", patient);

            var count = this.patientsService.GetPatientsCount();

            Assert.Equal(2, count);
        }

        [Fact]
        public void GetEmailByPatientIdShouldReturnCorrectEmail()
        {
            var email = "test@test.com";
            var user = new ApplicationUser() { UserName = "test", Email = email };

            var patient = new Patient()
            {
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                ImageUrl = "test",
                Town = new Town() { Name = "test" },
                BirthDate = DateTime.UtcNow,
                UserId = user.Id,
                User = user,
            };

            this.listOfUsers.Add(user);
            this.list.Add(patient);

            var actualEmail = this.patientsService.GetPatientEmailByPatientId(patient.Id);

            Assert.Equal(email, actualEmail);
        }

        [Fact]
        public async Task GetPatientByIdShouldGetCorrectPatient()
        {
            var patient = new Patient()
            {
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                ImageUrl = "test",
                Town = new Town() { Name = "test" },
                BirthDate = DateTime.UtcNow,
            };

            await this.patientsService.AddPatientToDb("test", patient);

            var patientFromService = this.patientsService.GetPatient<PatientViewModel>(patient.Id);

            Assert.Equal(patient.LastName, patientFromService.LastName);
            Assert.Equal(patient.BirthDate, patientFromService.BirthDate);
        }
    }
}
