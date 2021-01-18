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
            return new JsonResult(_dataContext.CalendarItems.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Post(CalendarItem CalendarItem)
        {
            ActionResult result;
            CalendarItem calendarItem = _dataContext.CalendarItems.FirstOrDefault(n => n.Id == CalendarItem.Id);
            if (calendarItem == null)
            {
                _dataContext.CalendarItems.Add(CalendarItem);
            }
            else
            {
                calendarItem.StartDate = CalendarItem.StartDate;
                calendarItem.EndDate = CalendarItem.EndDate;
                calendarItem.Title = CalendarItem.Title;
                calendarItem.Description = CalendarItem.Description;
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
