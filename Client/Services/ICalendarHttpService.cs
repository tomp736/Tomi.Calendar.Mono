using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Client.Services
{
    public interface ICalendarHttpService
    {
        Task<CalendarItem[]> GetCalendarItemsAsync();
        Task<CalendarItem> GetCalendarItemAsync(Guid calendarItemId);
        Task Save(CalendarItem calendarItemModel);
        Task Delete(CalendarItem calendarItemModel);
    }
}
