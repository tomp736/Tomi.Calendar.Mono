using NodaTime;
using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CalendarItemDto
    {
        public Guid Id { get; set; }

        public LocalDate? StartDate { get; set; }
        public LocalDate? EndDate { get; set; }

        public LocalTime? StartTime { get; set; }
        public LocalTime? EndTime { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
