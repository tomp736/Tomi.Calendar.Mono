using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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
            IAccessTokenProvider accessTokenProvider,
            ILogger<NotificationHubService> logger,
            IJSRuntime jsRuntime)
        {

            _notificationApiInterop = new NotificationJsInterop(jsRuntime);
            _logger = logger;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/notificationhub"), options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        AccessTokenResult accessTokenResult = await accessTokenProvider.RequestAccessToken();
                        AccessToken accessToken;
                        if(accessTokenResult.TryGetToken(out accessToken))
                        {
                            return accessToken.Value;
                        }
                        return "";
                    };
                })
                .Build();

            _hubConnection.On<string, string, string>("ReceiveNotification",
                async (title, description, iconurl) =>
                {
                    _logger.LogInformation($"{title}");
                    await ShowNotification(title, description, iconurl);
                });
        }

        public async Task Start()
        {
            await _hubConnection.StartAsync();
        }

        public async Task Stop()
        {
            await _hubConnection.StopAsync();
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

        public async Task ShowNotification(string title, string description, string iconurl)
        {
            await _notificationApiInterop.Notify(title, description, iconurl);
        }
    }
}
