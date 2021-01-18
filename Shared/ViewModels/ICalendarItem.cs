using System;

namespace Tomi.Calendar.Mono.Shared
{
    public interface ICalendarItem
    {
        DateTime EndDate { get; set; }
        DateTime StartDate { get; set; }
        string Title { get; set; }
    }
}