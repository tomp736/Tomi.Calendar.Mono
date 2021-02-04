using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System.Data;
using System.Threading.Tasks;
using Tomi.Calendar.Proto.CodeFirst;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class GrpcDataTableServiceClient
    {
        private GrpcChannel _gRpcChannel;
        public GrpcDataTableServiceClient(GrpcChannel gRpcChannel)
        {
            _gRpcChannel = gRpcChannel;
        }

        public async ValueTask<DataTableResult> GetDataTable(DataTableRequest dataTableRequest)
        {
            var client = _gRpcChannel.CreateGrpcService<IDataTableService>();
            return await client.GetDataTable(dataTableRequest);
        }
    }
}
