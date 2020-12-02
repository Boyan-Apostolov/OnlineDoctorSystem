using System.Collections.ObjectModel;

namespace OnlineDoctorSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Models;

    public class TownsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Towns.Any())
            {
                return;
            }

            var towns = new List<Tuple<string, double, double>>
                {
                    new Tuple<string, double, double>("Благоевград", 42.0100,23.0600),
                    new Tuple<string, double, double>("Бургас",42.510578, 27.461014),
                    new Tuple<string, double, double>("Варна",43.204666, 27.910543),
                    new Tuple<string, double, double>("Велико Търново",43.075672, 25.617151),
                    new Tuple<string, double, double>("Враца",43.21,23.5625),
                    new Tuple<string, double, double>("Габрово",42.87472, 25.33417),
                    new Tuple<string, double, double>("Добрич",43.56667, 27.83333),
                    new Tuple<string, double, double>("Кърджали",41.65, 25.36667),
                    new Tuple<string, double, double>("Кюстендил",42.28389, 22.69111),
                    new Tuple<string, double, double>("Монтана",43.4125, 23.225),
                    new Tuple<string, double, double>("Пазарджик",42.2, 24.33333),
                    new Tuple<string, double, double>("Перник",42.6, 23.03333),
                    new Tuple<string, double, double>("Плевен",43.41667, 24.61667),
                    new Tuple<string, double, double>("Пловдив",42.15, 24.75),
                    new Tuple<string, double, double>("Разград",43.53333, 26.51667),
                    new Tuple<string, double, double>("Русе",43.85639, 25.97083),
                    new Tuple<string, double, double>("Силистра",44.11667, 27.26667),
                    new Tuple<string, double, double>("Сливен",42.68583, 26.32917),
                    new Tuple<string, double, double>("Смолян",41.58528, 24.69194),
                    new Tuple<string, double, double>("София",42.698334, 23.319941),
                    new Tuple<string, double, double>("Стара Загора",42.43278, 25.64194),
                    new Tuple<string, double, double>("Търговище",43.2512, 26.57215),
                    new Tuple<string, double, double>("Хасково",41.93415, 25.55557),
                    new Tuple<string, double, double>("Шумен",43.27064, 26.92286),
                    new Tuple<string, double, double>("Ямбол",42.48333, 26.5),
                };

            foreach (var town in towns)
            {
                await dbContext.Towns.AddAsync(new Town() { Name = town.Item1, Latitude = town.Item2, Longitude = town.Item3 });
            }
        }
    }
}
