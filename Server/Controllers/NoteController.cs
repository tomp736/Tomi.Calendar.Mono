using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get(Guid id)
        {
            Note item = await _dataContext.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if(item != null)
            {
                return new JsonResult(item);
            }
            else
            {
                return new NotFoundObjectResult(id);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(NoteDto noteDto)
        {
            Note note = await _dataContext.Notes.FirstOrDefaultAsync(n => n.Id == noteDto.Id);
            if (note == null)
            {
                _dataContext.Notes.Add(new Note()
                {
                    Id = noteDto.Id,
                    Title = noteDto.Title,
                    Content = noteDto.Content,
                    CreateDate = noteDto.CreateDate.GetValueOrDefault(DateTime.Now)
                });
            }
            else
            {
                note.Title = noteDto.Title;
                note.Content = noteDto.Content;
            }
            int rowsAffected = await _dataContext.SaveChangesAsync();

            return new OkResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            ActionResult result;
            Note note = await _dataContext.Notes.FirstOrDefaultAsync(n => n.Id == id);
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
