using System.Threading.Tasks;

namespace Tomi.Notification.Core
{
    public interface INotificationPush
    {
        Task NotifyClient(INotify notify);
    }
}
