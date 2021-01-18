using System;
using System.Collections.Generic;
using System.Linq;

namespace Tomi.Calendar.Mono.Shared
{
    public static class CalendarItemExtensions
    {
        public static IEnumerable<CalendarItem> GetCalendarItems(this IEnumerable<CalendarItem> calendarItems, DateTime startDate, DateTime endDate)
        {
            return calendarItems.Where(calendarItem =>
                    calendarItem.EndDate.Date >= startDate.Date &&
                    calendarItem.StartDate.Date <= endDate.Date);
        }

        public static IEnumerable<CalendarItem> GetCalendarItems(this IEnumerable<CalendarItem> calendarItems, DateTime date)
        {
            return calendarItems.GetCalendarItems(date, date);
        }
    }
}
