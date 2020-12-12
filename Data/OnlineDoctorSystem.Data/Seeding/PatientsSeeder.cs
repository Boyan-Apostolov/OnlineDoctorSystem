namespace OnlineDoctorSystem.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;

    public class PatientsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedPatientAsync(userManager, $"patient@patient.com", dbContext);
        }

        private static async Task SeedPatientAsync(UserManager<ApplicationUser> userManager, string username, ApplicationDbContext dbContext)
        {
            Random r = new Random();
            var user = new ApplicationUser() { UserName = username, Email = username, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "Patient123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.PatientRoleName);
                if (user.Patient == null)
                {
                    var num = r.Next(0, 3);
                    user.Patient = new Patient()
                    {
                        FirstName = NamesLists.MaleFirstNames[num],
                        LastName = NamesLists.MaleLastNames[num],
                        Town = dbContext.Towns.Skip(num + 5).FirstOrDefault(),
                        Phone = $"09487{num}5563",
                        ImageUrl =
                            "https://res.cloudinary.com/du3ohgfpc/image/upload/v1606322301/jrtza0zytvwqeqihpg1m.png",
                        BirthDate = DateTime.UtcNow,
                        Gender = Gender.Male,
                    };
                }
            }
        }
    }
}
