using System.Collections.Generic;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Store.State
{
    public record CalendarState : RootState
    {
        public IEnumerable<CalendarItemDto>? CalendarItems { get; init; }
        public bool CalendarItemsLoaded => CalendarItems != null;

        public CalendarItemDto? CurrentCalendarItem { get; init; }

        public CalendarSplashSettings CalendarSplashSettings { get; init; }
    }
}
