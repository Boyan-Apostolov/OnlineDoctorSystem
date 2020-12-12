namespace OnlineDoctorSystem.Services.Data.ContactSubmission
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    public interface IContactSubmissionService
    {
        Task AddSubmissionToDb(ContactSubmissionViewModel model);

        IEnumerable<ContactSubmissionViewModel> GetAllSubmissions();
    }
}
