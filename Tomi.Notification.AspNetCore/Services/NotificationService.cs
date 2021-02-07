using Microsoft.AspNetCore.SignalR;
using System.Timers;
using Tomi.Notification.AspNetCore.Hubs;
using Tomi.Notification.Core;

namespace Tomi.Notification.AspNetCore.Services
{
    public class NotificationService
    {
        public readonly static Timer _timer = new Timer();
        private IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            _timer.Interval = 10000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            NotifyAll(new NotificationItem()
            {
                Title = "timer elapsed"
            });
        }

        public void NotifyUser(string user, INotify notify)
        {
            _hubContext.Clients.User(user).SendAsync(nameof(NotificationHub.PushNotification), notify);
        }

        public void NotifyAll(INotify notify)
        {
            _hubContext.Clients.All.SendAsync(nameof(NotificationHub.PushNotification), notify);
        }
    }
}
