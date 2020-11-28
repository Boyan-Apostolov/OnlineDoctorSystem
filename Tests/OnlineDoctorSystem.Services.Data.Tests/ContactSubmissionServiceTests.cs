namespace OnlineDoctorSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Moq;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Common.Repositories;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.ContactSubmission;
    using OnlineDoctorSystem.Web.ViewModels.Contacts;
    using Xunit;

    public class ContactSubmissionServiceTests
    {
        [Fact]
        public async Task AddingSubmissionShouldAddItToTheDB()
        {
            var list = new List<OnlineDoctorSystem.Data.Models.ContactSubmission>();

            var mockRepo = new Mock<IRepository<OnlineDoctorSystem.Data.Models.ContactSubmission>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<OnlineDoctorSystem.Data.Models.ContactSubmission>())).Callback(
                (OnlineDoctorSystem.Data.Models.ContactSubmission submission) => list.Add(submission));
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
