namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.ContactSubmission;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    public class ContactsController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly IContactSubmissionService submissionService;

        public ContactsController(
            IEmailSender emailSender,
            IContactSubmissionService submissionService)
        {
            this.emailSender = emailSender;
            this.submissionService = submissionService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactSubmissionInputModel model)
        {
            var errors = this.ModelState.Values.ToList();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.submissionService.AddSubmissionToDb(model);

            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemOwnerEmail,
                $"{model.Name} -> {model.Email}",
                GlobalConstants.SystemAdminEmail,
                $"{model.Title} -> {model.Email}",
                model.Content);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
