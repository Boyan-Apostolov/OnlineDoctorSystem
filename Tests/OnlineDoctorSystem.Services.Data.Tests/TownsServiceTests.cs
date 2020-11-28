namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.Towns;
    using Xunit;

    public class TownsServiceTests
    {
        private readonly EfDeletableEntityRepository<Town> repository;
        private readonly ITownsService townsService;

        public TownsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repo = new EfDeletableEntityRepository<Town>(new ApplicationDbContext(options.Options));
            var service = new TownsService(repo);

            this.townsService = service;
            this.repository = repo;
        }

        [Fact]
        public async Task GetSpecialtyByIdShouldReturnTheCorrectSpecialty()
        {
            await this.repository.AddAsync(new Town() { Name = "TestTown" });
            await this.repository.SaveChangesAsync();

            var town = this.townsService.GetTownById(1);

            Assert.Equal(1, town.Id);
            Assert.Equal("TestTown", town.Name);
        }

        [Fact]
        public async Task GetAllSpecialtiesShouldReturnTheCorrectNumberOfSpecialties()
        {
            await this.repository.AddAsync(new Town() { Name = "TestTown1" });
            await this.repository.AddAsync(new Town() { Name = "TestTown2" });
            await this.repository.AddAsync(new Town() { Name = "TestTown3" });
            await this.repository.SaveChangesAsync();

            var specialtiesCount = this.townsService.GetAllTowns().Count();

            Assert.Equal(3, 3);
        }
    }
}
