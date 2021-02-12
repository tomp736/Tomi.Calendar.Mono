using NodaTime;
using System;
using System.Collections.Generic;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CreateOrUpdateCalendarItemDto
    {
        public CreateOrUpdateCalendarItemDto(string title, string description, LocalDate startDate, LocalDate endDate, LocalTime startTime, LocalTime endTime, IEnumerable<Guid> tagIds, IEnumerable<Guid> noteIds)
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            StartTime = startTime;
            EndTime = endTime;
            TagIds = tagIds;
            NoteIds = noteIds;
        }

        public string Title { get; init; }
        public string Description { get; init; }
        public LocalDate StartDate { get; init; }
        public LocalDate EndDate { get; init; }
        public LocalTime StartTime { get; init; }
        public LocalTime EndTime { get; init; }

        public IEnumerable<Guid> TagIds { get; init; }
        public IEnumerable<Guid> NoteIds { get; init; }
    }
}
