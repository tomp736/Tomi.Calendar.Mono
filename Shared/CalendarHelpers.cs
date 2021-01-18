using System;
using System.Collections.Generic;
using System.Globalization;

namespace Tomi.Calendar.Mono.Shared
{
    public static class CalendarHelpers
    {
        public static string GetMonthName(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        }

        public static string GetDayName(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek);
        }

        public static DateTime GetStartDateOfWeek(DateTime dateTime, DayOfWeek startDateOfWeek)
        {
            dateTime = dateTime.Date;
            while (dateTime.DayOfWeek != startDateOfWeek)
            {
                dateTime = dateTime.AddDays(-1);
            }
            return dateTime;
        }

        public static DateTime GetEndDateOfWeek(DateTime dateTime, DayOfWeek startDateOfWeek)
        {
            dateTime = dateTime.Date;
            while (dateTime.AddDays(1).DayOfWeek != startDateOfWeek)
            {
                dateTime = dateTime.AddDays(1);
            }
            return dateTime;
        }

        public static IEnumerable<DateTime> CalendarDaysInView(DateTime startDate, DateTime endDate)
        {
            DateTime currentDate = startDate;
            while (currentDate <= endDate)
            {
                yield return currentDate;
                currentDate = currentDate.AddDays(1);
            }
        }
    }
}
