using System.Text.Json.Serialization;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Models
{
    public class ApplicationUserTag
    {
        public int TagKey { get; set; }

        [JsonIgnore]
        public Tag Tag { get; set; }

        public string UserKey { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
