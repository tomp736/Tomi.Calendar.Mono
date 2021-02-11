using ProtoBuf.Grpc.Client;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services.Grpc;
using Tomi.Calendar.Mono.Client.Services.Rest;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class CalendarDataService
    {
        private CalendarHttpService _calendarHttpService;
        private CalendarGrpcService _calendarGrpcService;

        public CalendarDataService(CalendarGrpcService calendarGrpcService, CalendarHttpService calendarHttpService)
        {
            _calendarGrpcService = calendarGrpcService;
            _calendarHttpService = calendarHttpService;
        }

        public async ValueTask<GetCalendarItemsResponse> GetCalendarItems(GetCalendarItemsRequest getCalendarItemsRequest)
        {
            return await _calendarGrpcService.GetCalendarItems(getCalendarItemsRequest);
        }

        public async ValueTask<SaveCalendarItemsResponse> SaveCalendarItems(SaveCalendarItemsRequest saveCalendarItemsRequest)
        {
            return await _calendarGrpcService.SaveCalendarItems(saveCalendarItemsRequest);
        }

        public async ValueTask<DeleteCalendarItemsResponse> DeleteCalendarItems(DeleteCalendarItemsRequest deleteCalendarItemsRequest)
        {
            return await _calendarGrpcService.DeleteCalendarItems(deleteCalendarItemsRequest);
        }


        public async ValueTask<TagDto> GetTagAsync(Guid tagId)
        {
            return await _calendarHttpService.GetTagAsync(tagId);
        }
        public async ValueTask<TagDto[]> GetTagsAsync()
        {
            return await _calendarHttpService.GetTagsAsync();
        }
        public async ValueTask SaveTag(TagDto tagDto)
        {
            await _calendarHttpService.Save(tagDto);
        }
        public async ValueTask DeleteTag(Guid id)
        {
            await _calendarHttpService.DeleteTag(id);
        }


        public async ValueTask<NoteDto> GetNoteAsync(Guid noteId)
        {
            return await _calendarHttpService.GetNoteAsync(noteId);
        }
        public async ValueTask<NoteDto[]> GetNotesAsync()
        {
            return await _calendarHttpService.GetNotesAsync();
        }
        public async ValueTask SaveNote(NoteDto NoteDto)
        {
            await _calendarHttpService.Save(NoteDto);
        }
        public async ValueTask DeleteNote(Guid id)
        {
            await _calendarHttpService.DeleteNote(id);
        }
    }
}
