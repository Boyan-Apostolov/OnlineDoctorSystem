using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OnlineDoctorSystem.Web.ViewModels.Contacts;

namespace OnlineDoctorSystem.Services.Data.ContactSubmission
{
    public interface IContactSubmissionService
    {
        Task AddSubmissionToDb(ContactSubmissionViewModel model);
    }
}
