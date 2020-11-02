using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Data.Seeding
{
    public class TownsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Towns.Any())
            {
                return;
            }
            var townNames = new string[]
            {
                "Благоевград",
                "Бургас",
                "Варна",
                "Велико Търново",
                "Враца",
                "Габрово",
                "Добрич",
                "Кърджали",
                "Кюстендил",
                "Монтана",
                "Пазарджик",
                "Перник",
                "Плевен",
                "Пловдив",
                "Разград",
                "Русе",
                "Силистра",
                "Сливен",
                "Смолян",
                "София град",
                "София област",
                "Стара Загора",
                "Търговище",
                "Хасково",
                "Шумен",
                "Ямбол"
            };
            foreach (var townName in townNames)
            {
                await dbContext.Towns.AddAsync(new Town() { TownName = townName });
            }
        }
    }
}
