using Fluxor;
using System;
using System.Drawing;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Store.Features.CalendarItem
{
    public class CalendarItemFeature : Feature<CalendarState>
    {
        public override string GetName() => typeof(CalendarItemFeature).Name;

        protected override CalendarState GetInitialState() =>
            new CalendarState
            {                
                CalendarItems = null,
                CurrentCalendarItem = null,
                StartDayOfWeek = DayOfWeek.Sunday,
                CalendarSplashSettings = new CalendarSplashSettings()
                {
                    PrimaryColor = "#f1f1f1",
                    SecondaryColor = "#ffffff",
                    StartDayOfWeek = DayOfWeek.Sunday
                }
            };
    }
}
