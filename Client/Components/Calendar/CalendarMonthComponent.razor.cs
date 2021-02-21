using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarMonthComponent : CalendarComponentBase
    {
        protected DateTime StartDate => CalendarHelpers.GetStartDateOfWeek(new DateTime(Date.Year, Date.Month, 1), CalendarState.Value.StartDayOfWeek);
        protected DateTime EndDate => StartDate.AddDays(34);

        [Parameter]
        public DayOfWeek StartDayOfWeek { get; set; }

        [Parameter]
        public RenderFragment<DateTime> CalendarDay { get; set; }
    }
}
