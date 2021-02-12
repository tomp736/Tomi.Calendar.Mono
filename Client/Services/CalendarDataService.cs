using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services.Grpc;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class CalendarDataService
    {
        private CalendarGrpcService _calendarGrpcService;

        public CalendarDataService(CalendarGrpcService calendarGrpcService)
        {
            _calendarGrpcService = calendarGrpcService;
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


        public async ValueTask<GetNotesResponse> GetNotes(GetNotesRequest getNotesRequest)
        {
            return await _calendarGrpcService.GetNotes(getNotesRequest);
        }

        public async ValueTask<SaveNotesResponse> SaveNotes(SaveNotesRequest saveNotesRequest)
        {
            return await _calendarGrpcService.SaveNotes(saveNotesRequest);
        }

        public async ValueTask<DeleteNotesResponse> DeleteNotes(DeleteNotesRequest deleteNotesRequest)
        {
            return await _calendarGrpcService.DeleteNotes(deleteNotesRequest);
        }


        public async ValueTask<GetTagsResponse> GetTags(GetTagsRequest getTagsRequest)
        {
            return await _calendarGrpcService.GetTags(getTagsRequest);
        }

        public async ValueTask<SaveTagsResponse> SaveTags(SaveTagsRequest saveTagsRequest)
        {
            return await _calendarGrpcService.SaveTags(saveTagsRequest);
        }

        public async ValueTask<DeleteTagsResponse> DeleteTags(DeleteTagsRequest deleteTagsRequest)
        {
            return await _calendarGrpcService.DeleteTags(deleteTagsRequest);
        }
    }
}
