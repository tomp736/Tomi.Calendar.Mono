using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CalendarItemDto
    {
        public Guid Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
