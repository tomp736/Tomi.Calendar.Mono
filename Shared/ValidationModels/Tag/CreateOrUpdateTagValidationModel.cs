using System;
using System.ComponentModel.DataAnnotations;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Shared.Dtos.CalendarItem
{
    public class CreateOrUpdateTagValidationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Must have a Description")]
        public string Description { get; set; }

        public string Color { get; set; }

        public CreateOrUpdateTagDto ToCreateOrUpdateTagDto()
        {
            return new CreateOrUpdateTagDto(Name, Description, Color);
        }
    }
}
