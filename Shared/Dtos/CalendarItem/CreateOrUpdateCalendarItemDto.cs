using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CreateOrUpdateCalendarItemDto
    {
        public CreateOrUpdateCalendarItemDto(string title, string description, DateTime startDate, DateTime endDate)
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
