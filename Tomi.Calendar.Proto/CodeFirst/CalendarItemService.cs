using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Proto.CodeFirst
{
    [ServiceContract(Name = "Mono.Calendar.CalendarItem")]
    public interface ICalendarItemService
    {
        ValueTask<GetCalendarItemsResponse> GetCalendarItems(GetCalendarItemsRequest request, CallContext context = default);
        ValueTask<SaveCalendarItemsResponse> SaveCalendarItems(SaveCalendarItemsRequest request, CallContext context = default);
        ValueTask<DeleteCalendarItemsResponse> DeleteCalendarItems(DeleteCalendarItemsRequest request, CallContext context = default);
    }

    [DataContract]
    public class GetCalendarItemsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? CalendarItemIds { get; set; }
    }

    [DataContract]
    public class GetCalendarItemsResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<CalendarItemDto>? CalendarItems { get; set; }
    }

    [DataContract]
    public class SaveCalendarItemsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<CalendarItemDto>? CalendarItems { get; set; }
    }

    [DataContract]
    public class SaveCalendarItemsResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<CalendarItemDto>? CalendarItems { get; set; }
    }

    [DataContract]
    public class DeleteCalendarItemsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? CalendarItemIds { get; set; }
    }

    [DataContract]
    public class DeleteCalendarItemsResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<CalendarItemDto>? CalendarItems { get; set; }
    }

    [DataContract]
    public class CalendarItemSurrogate
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

        // protobuf-net wants an implicit or explicit operator between the types
        public static implicit operator CalendarItemDto(CalendarItemSurrogate value)
        {
            if (value == null)
                return null;

            return new CalendarItemDto()
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                StartDate = value.StartDate,
                EndDate = value.EndDate,
                StartTime = value.StartTime,
                EndTime = value.EndTime
            };
        }

        public static implicit operator CalendarItemSurrogate(CalendarItemDto value)
        {
            if (value == null)
                return null;

            return new CalendarItemSurrogate()
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                StartDate = value.StartDate,
                EndDate = value.EndDate,
                StartTime = value.StartTime,
                EndTime = value.EndTime
            };
        }
    }
}
