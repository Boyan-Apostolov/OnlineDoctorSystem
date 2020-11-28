namespace OnlineDoctorSystem.Services.Data.ContactSubmission
{
    using System.Threading.Tasks;

    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;

    public class ContactSubmissionService : IContactSubmissionService
    {
        private readonly IRepository<OnlineDoctorSystem.Data.Models.ContactSubmission> submissionsRepository;

        public ContactSubmissionService(
            IRepository<OnlineDoctorSystem.Data.Models.ContactSubmission> submissionsRepository)
        {
            this.submissionsRepository = submissionsRepository;
        }

        public async Task AddSubmissionToDb(ContactSubmissionViewModel model)
        {
            var submission = new OnlineDoctorSystem.Data.Models.ContactSubmission()
            {
                Content = model.Content,
                Email = model.Email,
                Name = model.Name,
                Title = model.Title,
            };
            await this.submissionsRepository.AddAsync(submission);
            await this.submissionsRepository.SaveChangesAsync();
        }
    }
}
