using System;
using System.Collections.Generic;
using System.Linq;
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
using OnlineDoctorSystem.Web.ViewModels.Contacts;
using Xunit;

namespace OnlineDoctorSystem.Services.Data.Tests
{
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
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Patient>())).Callback((Patient patient) => this.list.Add(patient));

            mockRepoOfUser.Setup(x => x.All()).Returns(this.listOfUsers.AsQueryable());
            mockRepoOfUser.Setup(x => x.AddAsync(It.IsAny<ApplicationUser>())).Callback((ApplicationUser user) => this.listOfUsers.Add(user));

            var service = new PatientsService(mockRepo.Object, mockRepoOfUser.Object);
            this.patientsService = service;
        }

        //public PatientsServiceTests()
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    var patientRepo = new EfDeletableEntityRepository<Patient>(new ApplicationDbContext//(options.Options));
        //    var userRepo = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext//(options.Options));
        //    var service = new PatientsService(patientRepo, userRepo);
        //
        //    this.patientsService = service;
        //    this.repository = patientRepo;
        //}

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
        public async Task GetPatientEmailByUserIdShouldGetCorrectEmail()
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
            };
            this.listOfUsers.Add(user);
            await this.patientsService.AddPatientToDb(user.Id, patient);
            var actualEmail = await this.patientsService.GetPatientEmailByUserId(user.Id);

            Assert.Equal(email, actualEmail);
        }
    }
}
