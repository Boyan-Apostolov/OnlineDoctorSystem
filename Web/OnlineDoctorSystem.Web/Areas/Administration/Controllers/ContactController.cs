namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Services.Data.ContactSubmission;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    public class ContactController : AdministrationController
    {
        private readonly IContactSubmissionService contactSubmission;

        public ContactController(IContactSubmissionService contactSubmission)
        {
            this.contactSubmission = contactSubmission;
        }

        public IActionResult Index()
        {
            var viewModel = new ContactSubmissionViewModels()
            {
                Submissions = this.contactSubmission.GetAllSubmissions(),
            };

            return this.View(viewModel);
        }
    }
}
