using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Services.Data.Events;

namespace OnlineDoctorSystem.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    [IgnoreAntiforgeryToken]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventsService eventsService;

        public EventsController(ApplicationDbContext context, IEventsService eventsService)
        {
            _context = context;
            this.eventsService = eventsService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
        {
            //TODO: Only get user's events
            return _context.Events.Where(x => x.IsActive).ToList();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] CalendarEvent @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] CalendarEvent @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (this.eventsService.DeleteEventByIdAsync(id).Result)
            {
                return this.Ok();
            }

            return NotFound();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // PUT: api/Events/5/move
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveEvent([FromRoute] int id, [FromBody] EventMoveParams param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            @event.Start = param.Start;
            @event.End = param.End;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Events/5/color
        [HttpPut("{id}/color")]
        public async Task<IActionResult> SetEventColor([FromRoute] int id, [FromBody] EventColorParams param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            @event.Color = param.Color;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }

    public class EventMoveParams
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }

    public class EventColorParams
    {
        public string Color { get; set; }
    }
}
