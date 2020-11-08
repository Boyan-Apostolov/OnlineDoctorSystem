using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineDoctorSystem.Data.Common.Repositories;

namespace OnlineDoctorSystem.Services.Data.Users
{
    using System;
    using System.Collections.Generic;

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
            return await this.userManager.FindByNameAsync(username);
        }

        public async Task<bool> AddUserToRole(string username, string role)
        {
            var user = this.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            await this.userManager.AddToRoleAsync(user.Result, role);
            return true;
        }

        public async Task<bool> RemoveUserFromRole(string username, string role)
        {
            var user = this.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            await this.userManager.RemoveFromRoleAsync(user.Result, role);
            return true;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role)
        {
            var usersOfRole = await this.userManager.GetUsersInRoleAsync(role);

            return this.usersRepository.All().Where(x => usersOfRole.Any(u => u.Id == x.Id))
                .ToList();
        }
    }
}
