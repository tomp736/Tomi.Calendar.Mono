using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tomi.Calendar.Proto
{
    [DataContract]
    public class GetCalendarItemsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? CalendarItemIds { get; set; }
    }
}
