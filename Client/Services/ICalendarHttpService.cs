using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared;

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
