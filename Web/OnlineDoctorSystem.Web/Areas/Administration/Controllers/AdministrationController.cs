namespace OnlineDoctorSystem.Web.Areas.Administration.Controllers
{
    using AutoMapper.Configuration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Services.Data.Doctors;
    using OnlineDoctorSystem.Web.Controllers;
    using OnlineDoctorSystem.Web.ViewModels.Statistics;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        
    }
}
