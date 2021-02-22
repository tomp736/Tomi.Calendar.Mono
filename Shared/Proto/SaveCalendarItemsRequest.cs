using System.Collections.Generic;
using System.Runtime.Serialization;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Proto
{
    [DataContract]
    public class SaveCalendarItemsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<CalendarItemDto>? CalendarItems { get; set; }
    }
}
