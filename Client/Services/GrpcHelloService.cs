using Grpc.Net.Client;
using System.Threading.Tasks;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class GrpcHelloService
    {
        private GrpcChannel _gRpcChannel;
        public GrpcHelloService(GrpcChannel gRpcChannel)
        {
            _gRpcChannel = gRpcChannel;
        }

        public async Task<string> GetHelloReply(string name)
        {
            var client = new Greeter.GreeterClient(_gRpcChannel);
            HelloReply reply = await client.SayHelloAsync(new HelloRequest()
            {
                Name = name
            });

            return reply.Message;
        }
    }
}
