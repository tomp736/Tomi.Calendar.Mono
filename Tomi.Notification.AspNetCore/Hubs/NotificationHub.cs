using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Tomi.Notification.Core;

namespace Tomi.Notification.AspNetCore.Hubs
{
    public class NotificationHub : Hub<INotificationPush>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async void PushNotification(INotify notify)
        {
            await Clients.All.NotifyClient(notify);
        }
    }
}
