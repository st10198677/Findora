using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<Event>> Get() => _eventService.Get();

        [HttpGet("{id}")]
        public ActionResult<Event> Get(string id)
        {
            var eventObj = _eventService.Get(id);

            if (eventObj == null)
            {
                return NotFound();
            }

            return eventObj;
        }

        [HttpPost]
        public ActionResult<Event> Create(Event newEvent)
        {
            _eventService.Create(newEvent);
            return CreatedAtAction(nameof(Get), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Event updatedEvent)
        {
            var eventObj = _eventService.Get(id);

            if (eventObj == null)
            {
                return NotFound();
            }

            _eventService.Update(id, updatedEvent);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var eventObj = _eventService.Get(id);

            if (eventObj == null)
            {
                return NotFound();
            }

            _eventService.Remove(id);
            return NoContent();
        }
    }
}
