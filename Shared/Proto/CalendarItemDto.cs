using NodaTime;
using System;
using System.Runtime.Serialization;

namespace Tomi.Calendar.Proto
{
    [DataContract]
    public class CalendarItemDto
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
        [DataMember(Order = 2)]
        public string? Title { get; set; }
        [DataMember(Order = 3)]
        public string? Description { get; set; }
        [DataMember(Order = 4)]
        public LocalDate? StartDate { get; set; }
        [DataMember(Order = 5)]
        public LocalDate? EndDate { get; set; }
        [DataMember(Order = 6)]
        public LocalTime? StartTime { get; set; }
        [DataMember(Order = 7)]
        public LocalTime? EndTime { get; set; }
    }
}
