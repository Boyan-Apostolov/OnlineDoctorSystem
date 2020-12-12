namespace OnlineDoctorSystem.Web.Areas.Identity.Pages.Account
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using OnlineDoctorSystem.Data.Models;

    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with email '{email}'.");
            }

            this.Email = email;
            this.DisplayConfirmAccountLink = true;
            if (this.DisplayConfirmAccountLink)
            {
                var userId = await this.userManager.GetUserIdAsync(user);
                var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                this.EmailConfirmationUrl = this.Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId, code, returnUrl },
                    protocol: this.Request.Scheme);
            }

            return this.Page();
        }
    }
}
