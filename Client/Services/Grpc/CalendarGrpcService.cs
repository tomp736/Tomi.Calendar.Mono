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
            var client = _gRpcChannel.CreateGrpcService<ICalendarService>();
            return await client.GetCalendarItems(getCalendarItemsRequest);
        }

        public async ValueTask<SaveCalendarItemsResponse> SaveCalendarItems(SaveCalendarItemsRequest saveCalendarItemsRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<ICalendarService>();
            return await client.SaveCalendarItems(saveCalendarItemsRequest);
        }

        public async ValueTask<DeleteCalendarItemsResponse> DeleteCalendarItems(DeleteCalendarItemsRequest deleteCalendarItemsRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<ICalendarService>();
            return await client.DeleteCalendarItems(deleteCalendarItemsRequest);
        }
    }
}
