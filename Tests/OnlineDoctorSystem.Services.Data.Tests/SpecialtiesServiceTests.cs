namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.Specialties;
    using OnlineDoctorSystem.Services.Mapping;
    using OnlineDoctorSystem.Web.ViewModels.Home;
    using Xunit;

    public class SpecialtiesServiceTests
    {
        private readonly EfDeletableEntityRepository<Specialty> repository;
        private readonly ISpecialtiesService specialtiesService;

        public SpecialtiesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repo = new EfDeletableEntityRepository<Specialty>(new ApplicationDbContext(options.Options));
            var service = new SpecialtiesService(repo);

            AutoMapperConfig.RegisterMappings(
                typeof(SpecialtiesIndexViewModel).GetTypeInfo().Assembly);

            this.specialtiesService = service;
            this.repository = repo;
        }

        [Fact]
        public async Task GetSpecialtyByIdShouldReturnTheCorrectSpecialty()
        {
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty" });
            await this.repository.SaveChangesAsync();

            var specialty = this.specialtiesService.GetSpecialtyById(1);

            Assert.Equal(1, specialty.Id);
            Assert.Equal("TestSpecialty", specialty.Name);
        }

        [Fact]
        public async Task GetAllSpecialtiesShouldReturnTheCorrectNumberOfSpecialties()
        {
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty1" });
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty2" });
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty3" });
            await this.repository.SaveChangesAsync();

            var specialtiesCount = this.specialtiesService.GetAllSpecialties().Count();

            Assert.Equal(3, specialtiesCount);
        }

        [Fact]
        public async Task GetSpecialtiesCountShouldReturnTheCorrectAmount()
        {
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty1" });
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty2" });
            await this.repository.AddAsync(new Specialty() { Name = "TestSpecialty3" });
            await this.repository.SaveChangesAsync();

            var specialtiesCount = this.specialtiesService.GetSpecialtiesCount();
            Assert.Equal(3, specialtiesCount);
        }

        [Fact]
        public async Task GetAllSpecialtiesShouldReturnCorrectData()
        {
            var specialty = new Specialty() { Name = "TestSpecialty" };

            await this.repository.AddAsync(specialty);
            await this.repository.SaveChangesAsync();

            var specialtyFromService = this.specialtiesService.GetAllAsKeyValuePairs().First();

            Assert.Equal(specialty.Name, specialtyFromService.Value);
            Assert.Equal(specialty.Id, specialtyFromService.Key);
        }
    }
}
