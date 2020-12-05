using AutoMapper.Configuration;
using OnlineDoctorSystem.Services.Data.Doctors;
using OnlineDoctorSystem.Web.ViewModels.Statistics;

namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        
    }
}
