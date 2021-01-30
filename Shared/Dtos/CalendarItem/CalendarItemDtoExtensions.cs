using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public static class CalendarItemDtoExtensions
    {
        public static IEnumerable<CalendarItemDto> GetCalendarItems(this IEnumerable<CalendarItemDto> calendarItems, LocalDate startDate, LocalDate endDate)
        {
            return calendarItems.Where(calendarItem =>
                    calendarItem.EndDate.GetValueOrDefault(LocalDate.MaxIsoValue) >= startDate &&
                    calendarItem.StartDate.GetValueOrDefault(LocalDate.MinIsoValue) <= endDate);
        }

        public static IEnumerable<CalendarItemDto> GetCalendarItems(this IEnumerable<CalendarItemDto> calendarItems, LocalDate date)
        {
            return calendarItems.GetCalendarItems(date, date);
        }
    }
}
