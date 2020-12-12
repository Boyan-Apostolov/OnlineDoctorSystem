using Microsoft.AspNetCore.Mvc;
using OnlineDoctorSystem.Services.Data.ContactSubmission;
using OnlineDoctorSystem.Web.ViewModels.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
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
