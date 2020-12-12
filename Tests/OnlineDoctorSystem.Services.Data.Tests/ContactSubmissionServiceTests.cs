namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.ContactSubmission;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;
    using Xunit;

    public class ContactSubmissionServiceTests
    {
        [Fact]
        public async Task AddingSubmissionShouldAddItToTheDb()
        {
            var list = new List<ContactSubmission>();

            var mockRepo = new Mock<IRepository<ContactSubmission>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<ContactSubmission>())).Callback(
                (ContactSubmission submission) => list.Add(submission));
            var service = new ContactSubmissionService(mockRepo.Object);

            var content = "TestContent";

            await service.AddSubmissionToDb(new ContactSubmissionViewModel()
            {
                Content = content,
                Email = "test@test.com",
                Name = "Test",
                Title = "Test",
            });

            Assert.Contains(list, x => x.Content == content);
        }
    }
}
