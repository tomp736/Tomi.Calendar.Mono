using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly AppNpgSqlDataContext _dataContext;

        public TagController(AppNpgSqlDataContext dataContext, ILogger<TagController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(_dataContext.Tags.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Post(Tag Tag)
        {
            ActionResult result;
            Tag tag = _dataContext.Tags.FirstOrDefault(n => n.Id == Tag.Id);
            if (tag == null)
            {
                _dataContext.Tags.Add(Tag);
            }
            else
            {
                tag.Name = Tag.Name;
                tag.Description = Tag.Description;
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
            Tag tag = _dataContext.Tags.FirstOrDefault(n => n.Id == id);
            EntityEntry entityEntry = _dataContext.Tags.Remove(tag);
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
