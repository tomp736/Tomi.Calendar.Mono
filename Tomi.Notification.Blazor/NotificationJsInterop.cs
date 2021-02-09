using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Tomi.Notification.Blazor
{
    public class NotificationJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        private DotNetObjectReference<NotificationJsInterop> dotNetReference;

        public NotificationJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Tomi.Notification.Blazor/index.js").AsTask());
            dotNetReference = DotNetObjectReference.Create(this);
        }

        public async ValueTask<bool> HasPermissions()
        {
            var module = await moduleTask.Value;
            return (await module.InvokeAsync<string>("notificationPermissions")) == "granted";
        }

        public async Task AskForApproval()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("askForApproval");
        }

        public async Task Notify(string title, string text, string iconUrl)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("createNotification", title, text, iconUrl);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
            dotNetReference?.Dispose();
        }
    }
}
