namespace OnlineDoctorSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OnlineDoctorSystem.Common;
    using OnlineDoctorSystem.Data;
    using OnlineDoctorSystem.Data.Models;
    using OnlineDoctorSystem.Services.Data.Events;
    using OnlineDoctorSystem.Web.ViewModels.Events;

    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService eventsService;

        public EventsController(IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CalendarEvent>> GetEvents()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (this.User.IsInRole(GlobalConstants.DoctorRoleName))
            {
                return this.eventsService.GetDoctorsEvents(userId);
            }
            else if (this.User.IsInRole(GlobalConstants.PatientRoleName))
            {
                return this.eventsService.GetPatientsEvents(userId);
            }

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        public IActionResult DeleteEvent([FromRoute] int id)
        {
            if (this.eventsService.DeleteEventByIdAsync(id).Result)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        [HttpPut("{id}/move")]
        [Authorize(Roles = GlobalConstants.DoctorRoleName)]
        public async Task<IActionResult> MoveEvent([FromRoute] int id, [FromBody] EventMoveParams param)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.eventsService.MoveEvent(id, param.Start, param.End);

            return this.Ok();
        }

        [HttpPut("{id}/color")]
        public async Task<IActionResult> SetEventColor([FromRoute] int id, [FromBody] EventColorParams param)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.eventsService.ChangeEventColor(id, param.Color);

            return this.Ok();
        }
    }
}
