using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tomi.Calendar.Mono.Shared
{
    public class CalendarItemTag
    {
        public int CalendarItemKey { get; set; }

        [JsonIgnore]
        public CalendarItem CalendarItem { get; set; }

        public int TagKey { get; set; }

        [JsonIgnore]
        public Tag Tag { get; set; }
    }
}
