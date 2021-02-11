using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System.Threading.Tasks;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Services.Grpc
{
    public class CalendarGrpcService
    {
        private GrpcChannel _gRpcChannel;

        public CalendarGrpcService(GrpcChannel gRpcChannel)
        {
            _gRpcChannel = gRpcChannel;
        }

        public async ValueTask<GetCalendarItemsResponse> GetCalendarItems(GetCalendarItemsRequest getCalendarItemsRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<ICalendarItemService>();
            return await client.GetCalendarItems(getCalendarItemsRequest);
        }

        public async ValueTask<SaveCalendarItemsResponse> SaveCalendarItems(SaveCalendarItemsRequest saveCalendarItemsRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<ICalendarItemService>();
            return await client.SaveCalendarItems(saveCalendarItemsRequest);
        }

        public async ValueTask<DeleteCalendarItemsResponse> DeleteCalendarItems(DeleteCalendarItemsRequest deleteCalendarItemsRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<ICalendarItemService>();
            return await client.DeleteCalendarItems(deleteCalendarItemsRequest);
        }


        public async ValueTask<GetNotesResponse> GetNotes(GetNotesRequest getNotesRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<INoteService>();
            return await client.GetNotes(getNotesRequest);
        }

        public async ValueTask<SaveNotesResponse> SaveNotes(SaveNotesRequest saveNotesRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<INoteService>();
            return await client.SaveNotes(saveNotesRequest);
        }

        public async ValueTask<DeleteNotesResponse> DeleteNotes(DeleteNotesRequest deleteNotesRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<INoteService>();
            return await client.DeleteNotes(deleteNotesRequest);
        }
    }
}
