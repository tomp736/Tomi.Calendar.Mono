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
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Must have an End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today;
    }
}
