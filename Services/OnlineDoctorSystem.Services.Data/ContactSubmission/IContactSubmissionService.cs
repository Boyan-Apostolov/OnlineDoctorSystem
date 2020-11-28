namespace OnlineDoctorSystem.Services.Data.ContactSubmission
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    public interface IContactSubmissionService
    {
        Task AddSubmissionToDb(ContactSubmissionViewModel model);
    }
}
