namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.ViewModels.Doctors;

    public class ApprovalController : AdministrationController
    {
        private readonly IDoctorsService doctorsService;

        public ApprovalController(IDoctorsService doctorsService)
        {
            this.doctorsService = doctorsService;
        }

        public async Task<IActionResult> ApproveDoctor(string doctorId)
        {
            await this.doctorsService.ApproveDoctorAsync(doctorId);
            return this.RedirectToAction("GetUnconfirmedDoctors");
        }

        public async Task<IActionResult> DeclineDoctor(string doctorId)
        {
            await this.doctorsService.DeclineDoctorAsync(doctorId);
            return this.RedirectToAction("GetUnconfirmedDoctors");
        }

        public IActionResult GetUnconfirmedDoctors()
        {
            var viewModel = new ListingApprovalsViewModel()
            {
                Doctors = this.doctorsService.GetUnconfirmedDoctors<ApprovalDoctorViewModel>(),
            };
            return this.View(viewModel);
        }
    }
}
