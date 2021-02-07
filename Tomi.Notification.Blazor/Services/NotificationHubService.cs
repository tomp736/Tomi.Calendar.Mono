using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tomi.Notification.Core;

namespace Tomi.Notification.Blazor.Services
{
    public class NotificationHubService
    {
        private HubConnection _hubConnection;
        private NotificationJsInterop _notificationApiInterop;
        private ILogger<NotificationHubService> _logger;

        public NotificationHubService(
            NavigationManager navigationManager,
            ILogger<NotificationHubService> logger,
            IJSRuntime jsRuntime)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            _hubConnection.On<INotify>(nameof(INotificationPush.NotifyClient), async (notify) =>
            {
                await ShowNotification(notify);
            });

            _notificationApiInterop = new NotificationJsInterop(jsRuntime);
            _logger = logger;
        }

        public async ValueTask<bool> NotificationsAllowed()
        {
            _logger.LogInformation("NotificationsAllowed");
            return await _notificationApiInterop.HasPermissions();
        }

        public async Task RequestPermission()
        {
            _logger.LogInformation("RequestPermission");
            await _notificationApiInterop.AskForApproval();
        }

        public async Task ShowNotification(INotify notify)
        {
            await _notificationApiInterop.Notify(notify.Title, notify.Description, notify.ImgUrl);
        }
    }
}
