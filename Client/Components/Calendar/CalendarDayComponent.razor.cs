using System;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarDayComponent : CalendarComponentBase
    {
        protected string Heading => $"{Date.Month}/{Date.Day}";
        protected string ClassNames
        {
            get
            {
                string classnames = "";
                classnames += Date.AddDays(1).DayOfWeek == CalendarState.Value.StartDayOfWeek ? "last " : "";
                classnames += Date.Date.CompareTo(DateTime.Today) == 0 ? "today" : "";
                return classnames;
            }
        }
    }
}
