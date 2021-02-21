using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarMonthComponent : CalendarComponentBase
    {
        protected DateTime StartDate => CalendarHelpers.GetStartDateOfWeek(new DateTime(Date.Year, Date.Month, Date.Day), CalendarState.Value.StartDayOfWeek);
        protected DateTime EndDate => StartDate.AddDays(34);
        public DateTime CenterDate => CalendarHelpers.CalendarDaysInView(StartDate, EndDate).Skip(17).FirstOrDefault();
        public string Heading => $"{CalendarHelpers.GetMonthName(CenterDate)} - {CenterDate.Year}";

        [Parameter]
        public RenderFragment<DateTime> CalendarDay { get; set; }  

        [Parameter]
        public DayOfWeek StartDayOfWeek { get; set; }

        public void WheelHandler(WheelEventArgs wheelEventArgs)
        {
            int weeks = wheelEventArgs.DeltaY > 0 ? 1 : -1;
            AddWeeks(weeks);
        }
    }
}
