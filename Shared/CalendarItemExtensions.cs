using System;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Shared.Entities;

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
