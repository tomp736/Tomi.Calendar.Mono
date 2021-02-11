using System.Text.Json.Serialization;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Models
{
    public class ApplicationUserNote
    {
        public int NoteKey { get; set; }

        [JsonIgnore]
        public Note Note { get; set; }

        public string UserKey { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
