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
    }
}
