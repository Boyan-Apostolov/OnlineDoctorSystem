namespace OnlineDoctorSystem.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
        }

        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            return this.usersRepository.All().FirstOrDefault(x => x.UserName == username);
        }

        public async Task<bool> AddUserToRole(string username, string role)
        {
            var users = this.usersRepository.All().ToList();
            var user = await this.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            await this.userManager.AddToRoleAsync(user, role);
            return true;
        }
    }
}
