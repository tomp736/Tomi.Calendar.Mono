using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System.Data;
using System.Threading.Tasks;
using Tomi.Calendar.Proto.CodeFirst;
using System.Xml.Serialization;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Server
{
    public class GrpcDataTableService : IDataTableService
    {
        private readonly ILogger<GrpcGreeterService> _logger;
        public GrpcDataTableService(ILogger<GrpcGreeterService> logger)
        {
            _logger = logger;
        }

        public ValueTask<DataTableResult> GetDataTable(DataTableRequest request, CallContext context = default)
        {
            DataTable dt = DataTableHelper.GenerateTable(request.TableName, request.Columns, request.Rows);
            DataTableResult dtResult = new DataTableResult()
            {
                Result = dt
            };
            return ValueTask.FromResult(dtResult);
        }
    }
}
