using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalendarItemController : AuthorizedControllerBase
    {
        private readonly ILogger<CalendarItemController> _logger;
        private readonly AppNpgSqlDataContext _dataContext;

        public CalendarItemController(AppNpgSqlDataContext dataContext, ILogger<CalendarItemController> logger)
            : base(dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (CurrentUser != null)
            {
                var items = CurrentUser.UserCalendarItems.Select(n => n.CalendarItem);
                return new JsonResult(items.ToList());
            }
            return new NotFoundResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (CurrentUser != null)
            {
                CalendarItem item = CurrentUser.UserCalendarItems.FirstOrDefault(n => n.CalendarItem.Id == id)?.CalendarItem;
                return new JsonResult(item);
            }
            return new NotFoundObjectResult(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CalendarItem calendarItem)
        {
            ActionResult result = new OkResult();
            ApplicationUserCalendarItem userCalendarItem = CurrentUser.UserCalendarItems.FirstOrDefault(n => n.CalendarItem.Id == calendarItem.Id);
            try
            {
                if (userCalendarItem == null)
                {
                    _dataContext.ApplicationUserCalendarItem.Add(new ApplicationUserCalendarItem() { CalendarItem = calendarItem, User = CurrentUser });
                    _dataContext.CalendarItems.Add(calendarItem);
                }
                else
                {
                    userCalendarItem.CalendarItem.StartDate = calendarItem.StartDate;
                    userCalendarItem.CalendarItem.EndDate = calendarItem.EndDate;
                    userCalendarItem.CalendarItem.StartTime = calendarItem.StartTime;
                    userCalendarItem.CalendarItem.EndTime = calendarItem.EndTime;
                    userCalendarItem.CalendarItem.Title = calendarItem.Title;
                    userCalendarItem.CalendarItem.Description = calendarItem.Description;
                    userCalendarItem.CalendarItem.CalendarItemTags = calendarItem.CalendarItemTags;
                }

                int rowsAffected = await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                result = new StatusCodeResult(500);
            }
            return result;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            ActionResult result = new OkResult();

            if (CurrentUser != null)
            {
                CalendarItem calendarItem = CurrentUser.UserCalendarItems
                    .FirstOrDefault(n => n.CalendarItem.Id == id)?.CalendarItem;
                try
                {
                    if (calendarItem != null)
                    {
                        EntityEntry entityEntry = _dataContext.CalendarItems.Remove(calendarItem);
                        int rowsAffected = await _dataContext.SaveChangesAsync();
                    }
                    else
                    {
                        result = new NotFoundResult();
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    result = new StatusCodeResult(500);
                }
            }

            return result;
        }
    }
}
