using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CalendarItemController : ControllerBase
    {
        private readonly ILogger<CalendarItemController> _logger;
        private readonly AppNpgSqlDataContext _dataContext;

        public CalendarItemController(AppNpgSqlDataContext dataContext, ILogger<CalendarItemController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var items = _dataContext.CalendarItems.Include(item => item.CalendarItemTags);
            return new JsonResult(items.ToList());
        }

        [HttpGet("{id:guid}")]
        public ActionResult Get(Guid id)
        {
            var item = _dataContext.CalendarItems
                .Include(item => item.CalendarItemTags)
                .FirstOrDefault(n => n.Id == id);

            return new JsonResult(item);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CalendarItem CalendarItem)
        {
            ActionResult result;
            CalendarItem calendarItem = _dataContext.CalendarItems
                .Include(calendarItem => calendarItem.CalendarItemTags)
                .FirstOrDefault(n => n.Id == CalendarItem.Id);

            if (calendarItem == null)
            {
                _dataContext.CalendarItems.Add(CalendarItem);
            }
            else
            {
                calendarItem.StartDate = CalendarItem.StartDate;
                calendarItem.EndDate = CalendarItem.EndDate;
                calendarItem.StartTime = CalendarItem.StartTime;
                calendarItem.EndTime = CalendarItem.EndTime;
                calendarItem.Title = CalendarItem.Title;
                calendarItem.Description = CalendarItem.Description;
                calendarItem.CalendarItemTags = CalendarItem.CalendarItemTags;
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
            CalendarItem calendarItem = _dataContext.CalendarItems.FirstOrDefault(n => n.Id == id);
            EntityEntry entityEntry = _dataContext.CalendarItems.Remove(calendarItem);
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
