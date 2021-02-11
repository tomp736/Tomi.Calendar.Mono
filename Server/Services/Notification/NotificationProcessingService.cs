using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tomi.Notification.AspNetCore;
using Tomi.Notification.AspNetCore.Hubs;
using Tomi.Notification.Core;

namespace Tomi.Calendar.Mono.Server.Services.Notification
{
    internal class NotificationProcessingService : INotificationProcessingService
    {
        private readonly ILogger _logger;

        public NotificationProcessingService(ILogger<NotificationProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task ProcessNotifications(IHubContext<NotificationHub, INotificationHubService> hubContext, INotificationProcessingServiceDataProvider notificationProcessingServiceDataProvider, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessUserCalendarItemNotifications(hubContext, notificationProcessingServiceDataProvider);

                // delay until next HH:mm:00 instance
                await Task.Delay(TimeSpan.FromSeconds(60 - DateTime.Now.Second), stoppingToken);
            }
        }

        private async Task ProcessUserCalendarItemNotifications(IHubContext<NotificationHub, INotificationHubService> hubContext, INotificationProcessingServiceDataProvider notificationProcessingServiceDataProvider)
        {
            DateTime notificationDateTime = DateTime.Today.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
            var userNotificationItems = notificationProcessingServiceDataProvider.GetNotificationItems(notificationDateTime);

            foreach (var userNotificationItem in userNotificationItems)
            {
                await hubContext.Clients.Group($"user_{userNotificationItem.NotifyTypeName}")
                    .ReceiveNotification(userNotificationItem.Title, userNotificationItem.Description, userNotificationItem.IconUrl);
            }
        }
    }
}
