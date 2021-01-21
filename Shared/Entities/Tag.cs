using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tomi.Calendar.Mono.Shared
{
    public class Tag
    {
        [Key]
        public int Key { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<CalendarItemTag> CalendarItemTags { get; set; }
    }
}
