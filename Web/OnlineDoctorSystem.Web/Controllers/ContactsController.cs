using OnlineDoctorSystem.Services.Data.Emails;

namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.ContactSubmission;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    public class ContactsController : Controller
    {
        private readonly IContactSubmissionService submissionService;
        private readonly IEmailsService emailsService;

        public ContactsController(
            IContactSubmissionService submissionService,
            IEmailsService emailsService)
        {
            this.submissionService = submissionService;
            this.emailsService = emailsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactSubmissionInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.submissionService.AddSubmissionToDb(model);

            await this.emailsService.AddContactSubmissionEmailAsync(model.Name, model.Email, model.Title, model.Content);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
