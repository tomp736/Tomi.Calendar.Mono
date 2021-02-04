using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Server
{
    public class GrpcGreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GrpcGreeterService> _logger;
        public GrpcGreeterService(ILogger<GrpcGreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = request.Name,
                MessageType = "Hello"
            });
        }
    }
}
