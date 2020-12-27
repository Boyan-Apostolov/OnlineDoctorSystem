using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using OnlineDoctorSystem.Data.Common.Repositories;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services
{
    public class DoctorScraperService : IDoctorScraperService
    {
        private readonly IDeletableEntityRepository<Town> townsRepository;
        private readonly IDeletableEntityRepository<Specialty> specialtiesRepository;
        private readonly IDeletableEntityRepository<Doctor> doctorsRepository;

        private string BaseUrl = "https://superdoc.bg/lekari?sort=&page=";
        private IBrowsingContext context = new BrowsingContext();

        public DoctorScraperService(
            IDeletableEntityRepository<Town> townsRepository,
            IDeletableEntityRepository<Specialty> specialtiesRepository,
            IDeletableEntityRepository<Doctor> doctorsRepository)
        {
            this.townsRepository = townsRepository;
            this.specialtiesRepository = specialtiesRepository;
            this.doctorsRepository = doctorsRepository;
        }

        public async Task<int> Import(int pages = 1)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);

            var links = new List<string>();

            var addedDoctors = 0;

            for (int i = 0; i < pages; i++)
            {
                var url = string.Format(BaseUrl, i);
                links.AddRange(await this.GetLinks(url));
            }

            for (int j = 0; j < pages; j++)
            {
                try
                {
                    var doctor = await this.GetDoctor(links[j]);
                    await this.doctorsRepository.AddAsync(doctor);
                    addedDoctors++;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            await this.doctorsRepository.SaveChangesAsync();
            return addedDoctors;
        }

        private async Task<List<string>> GetLinks(string url)
        {
            var document = await this.context
                .OpenAsync(url);

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Тази страница не е намерена"))
            {
                throw new InvalidOperationException();
            }

            var indexElements = document.QuerySelectorAll(".search-results-holder > .search-result > .search-result-link")
                .ToArray();


            var links = new List<string>();
            foreach (var link in indexElements)
            {
                links.Add(link.GetAttribute("href"));
            }

            return links;
        }

        private async Task<Doctor> GetDoctor(string url)
        {
            var document = await this.context
                .OpenAsync(url);
            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Тази страница не е намерена"))
            {
                throw new InvalidOperationException();
            }

            //The doctor will be scrapped from third party website
            var isFromThirdParty = true;

            //Get original Url from 
            var originalUrl = url;

            //Get DoctorName
            var doctorName = document.QuerySelectorAll(".doctor-name > h1").Select(x => x.TextContent).First();

            //Get DoctorImage
            var imageUrl = document.QuerySelectorAll(".doctor-images > .gallery").First().GetAttribute("href");

            //Get DoctorSpecialty
            var specialty = document.QuerySelectorAll(".doctor-name > h2").Select(x => x.TextContent).First();

            //Get DoctorTown
            var town = document.QuerySelectorAll(".doctor-name > h3").Select(x => x.TextContent).First().Trim();

            //Get YearsOfPPractice
            var yearsOfPractice = document.QuerySelectorAll(".doctor-name > small").Select(x => x.TextContent).First().Trim().Substring(3, 2);

            //Get SmallInfo
            var smallInfo = document.QuerySelectorAll(".col-lg-10 > p").Select(x => x.TextContent).First().Trim();

            var doctorPersonalInfo = document.QuerySelectorAll(".doctor-description > p").Select(x => x.TextContent).ToList();

            //Get Education
            var education = doctorPersonalInfo[0];

            //Get Qualifications
            var qualifications = doctorPersonalInfo[1];

            //Get PreviousWork
            var previousWork = doctorPersonalInfo[2];

            //Get Email
            var doctorEmail = string.Empty;
            try
            {
                doctorEmail = document.QuerySelectorAll(".doctor-description > a").Select(x => x.TextContent).First()
                    .Trim();
            }
            catch (Exception e)
            {
                doctorEmail = "(няма предоставен имейл)";
            }

            //Is working with Children
            var isWorkingWithChildren = document.QuerySelectorAll(".badge-purple").ToList().Any();

            //Is working with NZOK
            var isWorkingWithNZOK = document.QuerySelectorAll(".badge-primary").ToList().Any();

            var doctor = new Doctor()
            {
                IsFromThirdParty = isFromThirdParty,
                LinkFromThirdParty = originalUrl,
                Name = doctorName,
                ImageUrl = imageUrl,
                YearsOfPractice = double.Parse(yearsOfPractice),
                SmallInfo = smallInfo,
                Qualifications = qualifications,
                Education = education,
                Biography = previousWork,
                ContactEmailFromThirdParty = doctorEmail,
                IsWorkingWithChildren = isWorkingWithChildren,
                IsWorkingWithNZOK = isWorkingWithNZOK,
                Town = await this.GetTownFromDb(town),
                Specialty = await this.GetSpecialtyFromDb(specialty),
                IsConfirmed = true,
                Phone = string.Empty,
            };
            return doctor;
        }

        private async Task<Town> GetTownFromDb(string townName)
        {
            var town = this.townsRepository.All().FirstOrDefault(x => x.Name == townName);
            if (town != null)
            {
                return town;
            }

            var newTown = new Town() { Name = townName };

            await this.townsRepository.AddAsync(newTown);
            await this.townsRepository.SaveChangesAsync();

            return newTown;
        }

        private async Task<Specialty> GetSpecialtyFromDb(string specialtyName)
        {
            var specialty = this.specialtiesRepository.All().FirstOrDefault(x => x.Name == specialtyName);
            if (specialty != null)
            {
                return specialty;
            }

            var newSpecialty = new Specialty() { Name = specialtyName };

            await this.specialtiesRepository.AddAsync(newSpecialty);
            await this.specialtiesRepository.SaveChangesAsync();

            return newSpecialty;
        }
    }
}
