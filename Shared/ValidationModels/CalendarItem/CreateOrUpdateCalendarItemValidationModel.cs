using NodaTime;
using System;
using System.ComponentModel.DataAnnotations;
using Tomi.Calendar.Proto;

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

        [Required(ErrorMessage = "Must have a Start Time")]
        public LocalTime StartTime { get; set; }

        [Required(ErrorMessage = "Must have an End Time")]
        public LocalTime EndTime { get; set; }

        public CalendarItemDto ToCalendarItemDto()
        {
            return new CalendarItemDto()
            {
                Title = Title,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                StartTime = StartTime,
                EndTime = EndTime
            };
        }
    }
}
