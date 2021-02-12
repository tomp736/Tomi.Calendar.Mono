using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Shared.Entities
{
    public class Tag
    {
        [Key]
        public int Key { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public ICollection<CalendarItemTag> CalendarItemTags { get; set; }

        public TagDto ToDto()
        {
            return new TagDto()
            {
                Id = Id,
                CreateDate = CreateDate,
                Name = Name,
                Description = Description,
                Color = Color
            };
        }
    }
}
