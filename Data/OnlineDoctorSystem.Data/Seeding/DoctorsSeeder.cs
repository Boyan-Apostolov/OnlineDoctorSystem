using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Models.Enums;
using OnlineDoctorSystem.Services.Data.Doctors;

namespace OnlineDoctorSystem.Data.Seeding
{
    public class DoctorsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedDoctorAsync(userManager, $"doctor@doctor.com", dbContext);

            for (int i = 1; i <= 4; i++)
            {
                //Test doctors
                await SeedDoctorAsync(userManager, $"doctor{i}@doctor.com", dbContext);
            }
        }

        private static async Task SeedDoctorAsync(UserManager<ApplicationUser> userManager, string username, ApplicationDbContext dbContext)
        {

            Random r = new Random();

            var result = await userManager.CreateAsync(new ApplicationUser() { UserName = username, Email = username, EmailConfirmed = true }, "Doctor123");
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(username);
                if (user.Doctor == null)
                {
                    var num = r.Next(0, 3);
                    user.Doctor = new Doctor()
                    {
                        Name = $"{NamesLists.maleFirstNames[num]} {NamesLists.maleLastNames[num]}",
                        Specialty = dbContext.Specialties.Skip(num + 3).FirstOrDefault(),
                        Town = dbContext.Towns.Skip(num + 5).FirstOrDefault(),
                        Phone = $"09987{num}5543",
                        ImageUrl =
                            "https://res.cloudinary.com/du3ohgfpc/image/upload/v1606322301/jrtza0zytvwqeqihpg1m.png",
                        BirthDate = DateTime.UtcNow,
                        Gender = Gender.Male,
                        YearsOfPractice = 10 + num,
                        IsWorkingWithNZOK = (num % 2 == 0),
                        IsWorkingWithChildren = true,
                        SmallInfo = $"Казвам се {NamesLists.maleFirstNames[num]} {NamesLists.maleLastNames[num]} и съм лекар от {10 + num} години.",
                        Education = $"Учил съм в Пловдивският медицински университет {8 + num} години.",
                        Qualifications = "Спечелил съм нобелова награда за откриване на ваксина срешу коронавируса",
                        Biography = "Работил съм като анестезиолог в софийската градска болница",
                        IsConfirmed = true,
                    };
                }
            }
        }
    }
}
