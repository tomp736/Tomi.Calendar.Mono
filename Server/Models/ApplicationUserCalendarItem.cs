using System.Text.Json.Serialization;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Models
{
    public class ApplicationUserCalendarItem
    {
        public int CalendarItemKey { get; set; }

        [JsonIgnore]
        public CalendarItem CalendarItem { get; set; }

        public string UserKey { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
