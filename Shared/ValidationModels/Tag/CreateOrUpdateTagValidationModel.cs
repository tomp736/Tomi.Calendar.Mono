using System;
using System.ComponentModel.DataAnnotations;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CreateOrUpdateTagValidationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Description")]
        public string Description { get; set; }
    }
}
