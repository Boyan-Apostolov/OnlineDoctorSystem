namespace OnlineDoctorSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class PatientsController : Controller
    {
        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
