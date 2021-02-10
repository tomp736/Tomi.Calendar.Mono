using Microsoft.AspNetCore.Components.Web;
using System;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarWeekComponent : CalendarComponentBase
    {
        protected DateTime StartDate => CalendarHelpers.GetStartDateOfWeek(new DateTime(Date.Year, Date.Month, Date.Day), CalendarState.Value.StartDayOfWeek);
        protected DateTime EndDate => StartDate.AddDays(6);

        public string Heading => $"{CalendarHelpers.GetMonthName(StartDate)} - {StartDate.Year}";

        public void WheelHandler(WheelEventArgs wheelEventArgs)
        {
            int weeks = wheelEventArgs.DeltaY < 0 ? 1 : -1;
            AddWeeks(weeks);
        }
    }
}
