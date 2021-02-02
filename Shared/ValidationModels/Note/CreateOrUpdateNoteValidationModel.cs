using System;
using System.ComponentModel.DataAnnotations;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CreateOrUpdateNoteValidationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have Content")]
        public string ContentText { get; set; }

        public string Content { get; set; }
    }
}
