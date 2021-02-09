using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Tomi.Notification.Core;

namespace Tomi.Notification.AspNetCore.Hubs
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        public async override Task OnConnectedAsync()
        {
            string email = Context.UserIdentifier;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{email}");

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async void PushNotification(string title, string description, string iconurl)
        {
            await Clients.All.ReceiveNotification(title, description, iconurl);
        }
    }

    //public class NameUserIdProvider : IUserIdProvider
    //{
    //    public string GetUserId(HubConnectionContext connection)
    //    {
    //        return connection.User?.Identity?.Name;
    //    }
    //}
}
