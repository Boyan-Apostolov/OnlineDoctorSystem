using System.Collections.Generic;
using System.Reflection;
using OnlineDoctorSystem.Services.Mapping;
using OnlineDoctorSystem.Web.ViewModels.Home;
using OnlineDoctorSystem.Web.ViewModels.Statistics;

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

            AutoMapperConfig.RegisterMappings(
                typeof(TownsIndexViewModel).GetTypeInfo().Assembly);

            this.townsService = service;
            this.repository = repo;
        }

        [Fact]
        public async Task GetTownByIdShouldReturnTheCorrectSpecialty()
        {
            await this.repository.AddAsync(new Town() { Name = "TestTown" });
            await this.repository.SaveChangesAsync();

            var town = this.townsService.GetTownById(1);

            Assert.Equal(1, town.Id);
            Assert.Equal("TestTown", town.Name);
        }

        [Fact]
        public async Task GetAllTownsAsKVPShouldReturnTheCorrectNumberOfSpecialties()
        {
            await this.repository.AddAsync(new Town() { Name = "TestTown1" });
            await this.repository.AddAsync(new Town() { Name = "TestTown2" });
            await this.repository.AddAsync(new Town() { Name = "TestTown3" });
            await this.repository.SaveChangesAsync();

            var count = this.townsService.GetAllAsKeyValuePairs().Count();
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetAllTownsAsKVPRecturnsCorrectData()
        {
            var town = new Town() { Name = "TestTown", Latitude = 0 };

            await this.repository.AddAsync(town);
            await this.repository.SaveChangesAsync();

            var townFromService = this.townsService.GetAllAsKeyValuePairs().First();
            Assert.Equal(town.Id, townFromService.Key);
            Assert.Equal(town.Name, townFromService.Value);
        }

        [Fact]
        public async Task GetAllTownsShouldReturnCorrectData()
        {
            var town = new Town() { Name = "TestTown", Latitude = 0 };

            await this.repository.AddAsync(town);
            await this.repository.SaveChangesAsync();
            var viewModel = new TownsIndexViewModel()
            {
                Id = town.Id,
                Name = town.Name
            };
            var townFromService = this.townsService.GetAllTowns<TownsIndexViewModel>().First();

            Assert.Equal(town.Name, townFromService.Name);
        }
    }
}
