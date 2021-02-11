using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.Note;

namespace Tomi.Calendar.Proto
{
    [ServiceContract(Name = "Mono.Calendar.Note")]
    public interface INoteService
    {
        ValueTask<GetNotesResponse> GetNotes(GetNotesRequest request, CallContext context = default);
        ValueTask<SaveNotesResponse> SaveNotes(SaveNotesRequest request, CallContext context = default);
        ValueTask<DeleteNotesResponse> DeleteNotes(DeleteNotesRequest request, CallContext context = default);
    }

    [DataContract]
    public class GetNotesRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? NoteIds { get; set; }
    }

    [DataContract]
    public class GetNotesResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<NoteDto>? Notes { get; set; }
    }

    [DataContract]
    public class SaveNotesRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<NoteDto>? Notes { get; set; }
    }

    [DataContract]
    public class SaveNotesResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<NoteDto>? Notes { get; set; }
    }

    [DataContract]
    public class DeleteNotesRequest
    {
        [DataMember(Order = 1)]
        public IEnumerable<Guid>? NoteIds { get; set; }
    }

    [DataContract]
    public class DeleteNotesResponse
    {
        [DataMember(Order = 1)]
        public IEnumerable<NoteDto>? Notes { get; set; }
    }

    [DataContract]
    public class NoteSurrogate
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
        [DataMember(Order = 2)]
        public DateTime? CreateDate { get; set; }
        [DataMember(Order = 3)]
        public string? Title { get; set; }
        [DataMember(Order = 4)]
        public string? Content { get; set; }

        public static implicit operator NoteDto(NoteSurrogate value)
        {
            if (value == null)
                return null;

            return new NoteDto()
            {
                Id = value.Id,
                CreateDate = value.CreateDate,
                Title = value.Title,
                Content = value.Content
            };
        }

        public static implicit operator NoteSurrogate(NoteDto value)
        {
            if (value == null)
                return null;

            return new NoteSurrogate()
            {
                Id = value.Id,
                CreateDate = value.CreateDate,
                Title = value.Title,
                Content = value.Content
            };
        }
    }
}
