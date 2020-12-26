namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Repositories;
    using OnlineDoctorSystem.Services.Data.Users;
    using Xunit;

    public class UsersServiceTests
    {
        private IUsersService usersService;
        private List<ApplicationUser> usersList;
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersServiceTests()
        {
            this.usersList = new List<ApplicationUser>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ApplicationUser>(this.usersList.FirstOrDefault(x => x.UserName == It.IsAny<string>())));

            this.usersService = new UsersService(this.usersRepository, userManager.Object);
        }

        [Fact]
        public void GetUserByUsernameWithFalseUserShouldReturnNull()
        {
            var user = this.usersService.GetUserByUsername("TestUser");
            Assert.Null(user);
        }

        [Fact]
        public async Task AddUserToRoleShouldAddTheUserToTheRole()
        {
            await this.usersRepository.AddAsync(new ApplicationUser { UserName = "user3", });
            await this.usersRepository.SaveChangesAsync();

            var role = "Admin";
            var isUserAddInRole = await this.usersService.AddUserToRole("user3", role);

            Assert.True(isUserAddInRole);
        }

        [Fact]
        public async Task AddNullUserToRoleShouldReturnFalse()
        {
            var role = "Admin";
            var isUserAddInRole = await this.usersService.AddUserToRole(string.Empty, role);

            Assert.False(isUserAddInRole);
        }
    }
}
