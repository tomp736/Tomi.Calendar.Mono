using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CreateOrUpdateCalendarItemValidationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Must have a Start Date")]
        public LocalDate StartDate { get; set; }

        [Required(ErrorMessage = "Must have an End Date")]
        public LocalDate EndDate { get; set; } 

        public LocalTime StartTime { get; set; }
        public LocalTime EndTime { get; set; }

        public List<Guid> TagIds { get; set; }
        public List<Guid> NoteIds { get; set; }
    }
}
