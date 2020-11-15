namespace OnlineDoctorSystem.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Data.Models.Enums;
    using OnlineDoctorSystem.Services.Data.Patients;
    using OnlineDoctorSystem.Services.Data.Towns;
    using OnlineDoctorSystem.Services.Data.Users;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    [AllowAnonymous]
    public class RegisterPatient : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterPatient> logger;
        private readonly IEmailSender emailSender;
        private readonly IUsersService usersService;
        private readonly ITownsService townsService;
        private readonly IPatientsService patientsService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public RegisterPatient(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterPatient> logger,
            IEmailSender emailSender,
            IUsersService usersService,
            ITownsService townsService,
            IPatientsService patientsService,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.usersService = usersService;
            this.townsService = townsService;
            this.patientsService = patientsService;
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Името е задължително")]
            [MinLength(3, ErrorMessage = "Името трябва да се състои от минимум 3 символа")]
            [MaxLength(30, ErrorMessage = "Името трябва да се състои от максимум 30 символа")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Фамилията е задължителна")]
            [MinLength(3, ErrorMessage = "Фамилията трябва да се състои от минимум 3 символа")]
            [MaxLength(30, ErrorMessage = "Фамилията трябва да се състои от максимум 30 символа")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Телефона е задължителен")]
            [Phone]
            public string Phone { get; set; }

            public int TownId { get; set; }

            [Required]
            [DataType(DataType.Date)]

            public DateTime BirthDate { get; set; }

            [Required]
            public Gender Gender { get; set; }

            [Required(ErrorMessage = "Полето е задължително")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MinLength(6, ErrorMessage = "Паролата трябва да се състои от минимум 6 символа")]
            [MaxLength(25, ErrorMessage = "Паролата трябва да се състои от максимум 25 символа")]
            [PasswordPropertyText]
            public string Password { get; set; }

            [Required]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Снимката е задължително")]
            [DataType(DataType.ImageUrl)]
            public IFormFile Image { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Patients/ThankYou");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!(this.Input.Image.FileName.EndsWith(".jpg")
                || this.Input.Image.FileName.EndsWith(".png")
                || this.Input.Image.FileName.EndsWith(".jpeg")))
            {
                this.ModelState.AddModelError("Image", "Invalid file type.");
            }

            if (this.Input.Password != this.Input.ConfirmPassword)
            {
                this.ModelState.AddModelError("Password", "Passwords do not match.");
            }

            if (this.ModelState.IsValid)
            {

                var cloudinaryAccount = this.configuration.GetSection("Cloudinary");
                CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
                    cloudinaryAccount["Cloud_Name"],
                    cloudinaryAccount["API_Key"],
                    cloudinaryAccount["API_Secret"]);

                var cloudinary = new Cloudinary(account);

                var file = this.Input.Image;

                var uploadResult = new ImageUploadResult();

                var imageUrl = "";

                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = file.OpenReadStream())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(file.Name, stream),
                                Transformation = new Transformation().Width(256).Height(256).Gravity("face").Radius("max").Border("2px_solid_white").Crop("thumb"),
                            };

                            uploadResult = cloudinary.Upload(uploadParams);
                        }
                    }

                    imageUrl = uploadResult.Uri.ToString();
                }
                var user = new ApplicationUser { UserName = this.Input.Email, Email = this.Input.Email };

                var patient = new Patient()
                {
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    Phone = this.Input.Phone,
                    Town = this.townsService.GetTownById(this.Input.TownId),
                    BirthDate = this.Input.BirthDate,
                    Gender = this.Input.Gender,
                    UserId = user.Id,
                    ImageUrl = imageUrl,
                };
                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new
                        {
                            area = "Identity",
                            userId = user.Id,
                            code = code,
                            returnUrl = returnUrl,
                        },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        htmlMessage: $"Моля потвърдете своя акаунт от тук <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'></a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await this.usersService.AddUserToRole(user.UserName, GlobalConstants.PatientRoleName);

                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        await this.patientsService.AddPatientToDb(user.Id, patient);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
