using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Tomi.Blazor.Notification
{
    public class NotificationApiInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        private DotNetObjectReference<NotificationApiInterop> dotNetReference;

        public NotificationApiInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Tomi.Blazor.Notification/index.js").AsTask());
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

        public async ValueTask<int> ScheduleNotification(long timeout, string title, string text, string iconUrl)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("scheduleNotification", dotNetReference, timeout, title, text, iconUrl);
        }

        public async Task CancelNotification(int jsHandle)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("cancelNotification", dotNetReference, jsHandle);
        }

        public Action<Guid> OnNotificationCreated { get; set; }
        [JSInvokable("NotificationCreated")]
        public void NotificationCreated(Guid id)
        {
            OnNotificationCreated?.Invoke(id);
        }


        public Action<int> OnNotificationScheduled { get; set; }
        [JSInvokable("NotificationScheduled")]
        public void NotificationScheduled(int jsHandle)
        {
            OnNotificationScheduled?.Invoke(jsHandle);
        }


        public Action OnNotificationCancelled { get; set; }
        [JSInvokable("NotificationCancelled")]
        public void NotificationCancelled()
        {
            OnNotificationCancelled?.Invoke();
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
