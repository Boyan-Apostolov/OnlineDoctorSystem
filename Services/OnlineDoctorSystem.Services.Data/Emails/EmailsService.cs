using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Services.Data.Emails
{
    using System;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Messaging;

    public class EmailsService : IEmailsService
    {
        private readonly IEmailSender emailSender;

        public EmailsService(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task ApproveDoctorEmailAsync(string doctorEmail)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                doctorEmail,
                "Вашият профил беше потвърден!",
                $"Вашият профил беше потвърден! Вече сте част от нашето семейство и наши пациенти могат да се свързват с вас!");
        }

        public async Task DeclineDoctorEmailAsync(string doctorEmail)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                doctorEmail,
                "Вашата заявка за профил беше отхвърлена!",
                $"Съжеляваме, но профилът Ви не покрива изискванията ни. Можете да виждате другите доктори, но вашият профил няма да може да бъде намерен от пациентите ни!");
        }

        public async Task AddConsultationEmailAsync(string patientFirstName, string patientLastName, string doctorEmail, DateTime date, TimeSpan? time)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"{patientFirstName} {patientLastName}",
                doctorEmail,
                "Имате нова заявка за консултация",
                $"Имате нова заявка за консултация от пациент {patientFirstName} {patientLastName} за {date.ToShortDateString()} от {time} часа. Моля потвърдете или отхвърлете заявката от сайта ни.");
        }

        public async Task ApproveConsultationEmailAsync(string patientEmail, DateTime date, TimeSpan time)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                patientEmail,
                "Вашата заявка беше одобрена!",
                $"Вашата заявка за консултация на {date.ToShortDateString()} от {time} беше одобрена. Доктора ще ви очаква :)");
        }

        public async Task DeclineConsultationEmailAsync(string patientEmail, DateTime date, TimeSpan time)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                patientEmail,
                "Вашата заявка беше отхвърлена!",
                $"Вашата заявка за консултация на {date.ToShortDateString()} от {time} беше отхвърлена от доктора ви, за повече информация, моля свържете се с него.");
        }

        public async Task DeleteEventEmailAsync(string patientEmail, DateTime date, TimeSpan time)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                "Онлайн-ДокторСистема",
                patientEmail,
                "Едно от предстоящите ве събития беше изтрито!",
                $"Събитието ви за {date.Date.ToShortDateString()} от {time} часа беше изтрито. За повече информация, моля свържете се с практикуващия лекар.");
        }

        public async Task MoveEventEmailAsync(string patientEmail, DateTime previousDate, DateTime currentDate)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                $"Админ на Онлайн-Доктор Системата",
                patientEmail,
                "Датата на вашата заявка беше променена!",
                $"Вашата консултация от {previousDate.ToShortDateString()} беше преместена на {currentDate.Date.ToShortDateString()}.");
        }

        public async Task AddContactSubmissionEmailAsync(string senderName, string senderEmail, string title, string content)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemOwnerEmail,
                $"{senderName} -> {senderEmail}",
                GlobalConstants.SystemAdminEmail,
                $"{title} -> {senderEmail}",
                content);
        }

        public async Task DoctorToPatientEmail(DoctorEmailViewModel model)
        {
            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemAdminEmail,
                model.DoctorEmail,
                model.PatientEmail,
                model.Title,
                $"Съобщение от {model.DoctorEmail}:  {model.Content}");
        }
    }
}
