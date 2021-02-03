using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Services.Data.Emails
{
    using System;
    using System.Threading.Tasks;

    public interface IEmailsService
    {
        public Task ApproveDoctorEmailAsync(string doctorEmail);

        public Task DeclineDoctorEmailAsync(string doctorEmail);

        public Task AddConsultationEmailAsync(string patientFirstName, string patientLastName, string doctorEmail, DateTime date, TimeSpan? time);

        public Task ApproveConsultationEmailAsync(string patientEmail, DateTime date, TimeSpan time);

        public Task DeclineConsultationEmailAsync(string patientEmail, DateTime date, TimeSpan time);

        public Task DeleteEventEmailAsync(string patientEmail, DateTime date, TimeSpan time);

        public Task MoveEventEmailAsync(string patientEmail, DateTime previousDate, DateTime currentDate);

        public Task AddContactSubmissionEmailAsync(string senderName, string senderEmail, string title, string content);

        public Task DoctorToPatientEmail(DoctorEmailViewModel mode);
    }
}
