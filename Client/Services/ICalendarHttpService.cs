using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Services
{
    public interface ICalendarHttpService
    {
        Task<CalendarItem[]> GetItemsAsync();
        Task<CalendarItem> GetItemAsync(Guid calendarItemId);
        Task Save(CalendarItem calendarItemModel);
        Task Delete(CalendarItem calendarItemModel);
    }
}
