namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Consultations;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Data.Specialties;
    using OnlineDoctorSystem.Services.Data.Towns;
    using OnlineDoctorSystem.Web.ViewModels.Statistics;

    public class StatisticsController : Controller
    {
        private readonly IDoctorsService doctorsService;
        private readonly ITownsService townsService;
        private readonly IPatientsService patientsService;
        private readonly IConsultationsService consultationsService;
        private readonly ISpecialtiesService specialtiesService;

        public StatisticsController(
            IDoctorsService doctorsService,
            ITownsService townsService,
            IPatientsService patientsService,
            IConsultationsService consultationsService,
            ISpecialtiesService specialtiesService)
        {
            this.doctorsService = doctorsService;
            this.townsService = townsService;
            this.patientsService = patientsService;
            this.consultationsService = consultationsService;
            this.specialtiesService = specialtiesService;
        }

        public IActionResult Index()
        {
            var viewModel = new StatsViewModel()
            {
                DoctorsCount = this.doctorsService.GetDoctorsCount(),
                Towns = this.townsService.GetAllTowns<TownViewModel>(),
                PatientsCount = this.patientsService.GetPatientsCount(),
                ConsultationsCount = this.consultationsService.GetConsultationsCount(),
                ReviewsCount = this.doctorsService.GetReviewsCount(),
                SpecialtiesCount = this.specialtiesService.GetSpecialtiesCount(),
            };
            return this.View(viewModel);
        }
    }
}
