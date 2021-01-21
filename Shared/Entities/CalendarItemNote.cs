using System.Text.Json.Serialization;

namespace Tomi.Calendar.Mono.Shared.Entities
{
    public class CalendarItemNote
    {
        public int CalendarItemKey { get; set; }

        [JsonIgnore]
        public CalendarItem CalendarItem { get; set; }

        public int NoteKey { get; set; }

        [JsonIgnore]
        public Note Note { get; set; }
    }
}
