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
