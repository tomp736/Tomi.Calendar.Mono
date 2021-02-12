using NodaTime;
using System;
using System.Collections.Generic;

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

        public IEnumerable<Guid> TagIds { get; set; }
        public IEnumerable<Guid> NoteIds { get; set; }
    }
}
