using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Tomi.Calendar.Proto
{
    [ServiceContract(Name = "Tomi.Calendar")]
    public interface ICalendarService
    {
        ValueTask<GetCalendarItemsResponse> GetCalendarItems(GetCalendarItemsRequest request, CallContext context = default);
        ValueTask<SaveCalendarItemsResponse> SaveCalendarItems(SaveCalendarItemsRequest request, CallContext context = default);
        ValueTask<DeleteCalendarItemsResponse> DeleteCalendarItems(DeleteCalendarItemsRequest request, CallContext context = default);
    }
}
