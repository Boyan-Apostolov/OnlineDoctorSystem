namespace OnlineDoctorSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Messaging;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    [Authorize]
    public class ContactsController : Controller
    {
        private readonly IEmailSender emailSender;

        public ContactsController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactSubmissionViewModel model)
        {
            var errors = this.ModelState.Values.ToList();

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.emailSender.SendEmailAsync(
                model.Email,
                model.Name,
                GlobalConstants.SystemEmail,
                model.Title,
                model.Content);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
