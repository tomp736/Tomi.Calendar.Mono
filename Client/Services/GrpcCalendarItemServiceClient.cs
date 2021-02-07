using Grpc.Core;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System.Data;
using System.Threading.Tasks;
using Tomi.Calendar.Proto.CodeFirst;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class GrpcCalendarItemServiceClient
    {
        private GrpcChannel _gRpcChannel;
        public GrpcCalendarItemServiceClient(GrpcChannel gRpcChannel)
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
    }
}
