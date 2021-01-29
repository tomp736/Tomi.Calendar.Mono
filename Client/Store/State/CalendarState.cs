using System;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Client.Store.State
{
    public record CalendarState : RootState
    {
        public IEnumerable<CalendarItemDto>? CalendarItems { get; init; }
        public bool CalendarItemsLoaded => CalendarItems != null && CalendarItems.Any();

        public IEnumerable<TagDto>? Tags { get; init; }
        public bool TagsLoaded => Tags != null && Tags.Any();

        public IEnumerable<NoteDto>? Notes { get; init; }
        public bool NotesLoaded => Notes != null && Notes.Any();

        public CalendarItemDto? CurrentCalendarItem { get; init; }
        public TagDto? CurrentTag { get; init; }
        public NoteDto? CurrentNote { get; init; }

        public DayOfWeek StartDayOfWeek { get; init; }
    }
}
