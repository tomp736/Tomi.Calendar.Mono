using System;
using System.Collections.Generic;
using System.Linq;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public static class CalendarItemDtoExtensions
    {
        public static IEnumerable<CalendarItemDto> GetCalendarItems(this IEnumerable<CalendarItemDto> calendarItems, DateTime startDate, DateTime endDate)
        {
            return calendarItems.Where(calendarItem =>
                    calendarItem.EndDate.GetValueOrDefault(DateTime.MaxValue).Date >= startDate.Date &&
                    calendarItem.StartDate.GetValueOrDefault(DateTime.MinValue).Date <= endDate.Date);
        }

        public static IEnumerable<CalendarItemDto> GetCalendarItems(this IEnumerable<CalendarItemDto> calendarItems, DateTime date)
        {
            return calendarItems.GetCalendarItems(date, date);
        }
    }
}
