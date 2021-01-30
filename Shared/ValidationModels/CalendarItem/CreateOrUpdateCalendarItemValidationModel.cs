using NodaTime;
using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
