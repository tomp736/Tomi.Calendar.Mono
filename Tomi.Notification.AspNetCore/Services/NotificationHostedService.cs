using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tomi.Notification.AspNetCore.Hubs;
using Tomi.Notification.Core;

namespace Tomi.Notification.AspNetCore.Services
{
    public class NotificationHostedService : BackgroundService
    {
        private IHubContext<NotificationHub, INotificationClient> _hubContext;
        private IServiceProvider _services { get; }

        public NotificationHostedService(IHubContext<NotificationHub, INotificationClient> hubContext, IServiceProvider services)
        {
            _hubContext = hubContext;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<INotificationProcessingService>();
                await scopedProcessingService.ProcessUserNotifications(_hubContext, stoppingToken);
            }
        }

        public void NotifyUser(string user, string title, string description, string iconurl)
        {
            _hubContext.Clients.Group($"user_{user}").ReceiveNotification(title, description, iconurl);
        }

        public void NotifyAll(string title, string description, string iconurl)
        {
            _hubContext.Clients.All.ReceiveNotification(title, description, iconurl);
        }
    }
}
