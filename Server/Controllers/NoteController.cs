using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly AppNpgSqlDataContext _dataContext;

        public NoteController(AppNpgSqlDataContext dataContext, ILogger<NoteController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(_dataContext.Notes.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Post(Note Note)
        {
            ActionResult result;
            Note note = _dataContext.Notes.FirstOrDefault(n => n.Id == Note.Id);
            if (note == null)
            {
                _dataContext.Notes.Add(Note);
            }
            else
            {
                note.Title = Note.Title;
                note.Content = Note.Content;
            }
            int rowsAffected = await _dataContext.SaveChangesAsync();
            if (rowsAffected == 1)
            {
                result = new OkResult();
            }
            else
            {
                result = new BadRequestResult();
            }
            return result;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            ActionResult result;
            Note note = _dataContext.Notes.FirstOrDefault(n => n.Id == id);
            EntityEntry entityEntry = _dataContext.Notes.Remove(note);
            int rowsAffected = await _dataContext.SaveChangesAsync();
            if (rowsAffected == 1)
            {
                result = new OkResult();
            }
            else
            {
                result = new NotFoundResult();
            }
            return result;
        }
    }
}
