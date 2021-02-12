using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Proto
{
    [ServiceContract(Name = "Mono.Calendar.Tag")]
    public interface ITagService
    {
        ValueTask<GetTagsResponse> GetTags(GetTagsRequest request, CallContext context = default);
        ValueTask<SaveTagsResponse> SaveTags(SaveTagsRequest request, CallContext context = default);
        ValueTask<DeleteTagsResponse> DeleteTags(DeleteTagsRequest request, CallContext context = default);
    }

    [DataContract]
    public class GetTagsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? TagIds { get; set; }
    }

    [DataContract]
    public class GetTagsResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<TagDto>? Tags { get; set; }
    }

    [DataContract]
    public class SaveTagsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<TagDto>? Tags { get; set; }
    }

    [DataContract]
    public class SaveTagsResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<TagDto>? Tags { get; set; }
    }

    [DataContract]
    public class DeleteTagsRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? TagIds { get; set; }
    }

    [DataContract]
    public class DeleteTagsResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<TagDto>? Tags { get; set; }
    }

    [DataContract]
    public class TagSurrogate
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
        [DataMember(Order = 2)]
        public DateTime CreateDate { get; set; }
        [DataMember(Order = 3)]
        public string Name { get; set; }
        [DataMember(Order = 4)]
        public string Description { get; set; }
        [DataMember(Order = 5)]
        public string Color { get; set; }

        public static implicit operator TagDto(TagSurrogate value)
        {
            if (value == null)
                return null;

            return new TagDto()
            {
                Id = value.Id,
                CreateDate = value.CreateDate,
                Name = value.Name,
                Description = value.Description,
                Color = value.Color
            };
        }

        public static implicit operator TagSurrogate(TagDto value)
        {
            if (value == null)
                return null;

            return new TagSurrogate()
            {
                Id = value.Id,
                CreateDate = value.CreateDate,
                Name = value.Name,
                Description = value.Description,
                Color = value.Color
            };
        }
    }
}
