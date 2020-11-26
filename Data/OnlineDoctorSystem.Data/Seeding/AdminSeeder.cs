using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Models.Enums;

namespace OnlineDoctorSystem.Data.Seeding
{
    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedDoctorAsync(userManager, $"admin@admin.com");
        }

        private static async Task SeedDoctorAsync(UserManager<ApplicationUser> userManager, string username)
        {
            var user = new ApplicationUser()
            {
                UserName = username,
                Email = username,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "Admin123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
