using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tomi.Blazor.Notification.Services
{
    public class NotificationService
    {
        private Dictionary<Guid, List<int>> _scheduledNotifications
            = new Dictionary<Guid, List<int>>();

        private NotificationApiInterop _notificationApiInterop;
        private ILogger<NotificationService> _logger;

        public NotificationService(IJSRuntime jsRuntime, ILogger<NotificationService> logger)
        {
            _notificationApiInterop = new NotificationApiInterop(jsRuntime);
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

        public async Task RegisterNotifications(IEnumerable<INotify> notifys)
        {
            foreach (INotify notify in notifys)
            {
                await RegisterNotification(notify);
            }
        }

        public async Task RegisterNotification(INotify notify)
        {
            await DeRegisterNotification(notify.Id);
            await RegisterNotifications(notify);
        }

        private async Task RegisterNotifications(INotify notify)
        {
            _scheduledNotifications[notify.Id] = new List<int>();
            DateTime currentDate = DateTime.Now;

            if (notify.StartDate.Date <= currentDate.Date && notify.EndDate.Date >= currentDate.Date)
            {
                // _logger.LogInformation($"Notification: {notify.Title} StartDate: {notify.StartDate} EndDate: {notify.EndDate} StartTime: {notify.StartTime.Hours} EndTime: {notify.EndTime.Hours} CurrentDate: {currentDate}");

                DateTime startDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, notify.StartTime.Hours, notify.StartTime.Minutes, notify.StartTime.Seconds);
                DateTime endDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, notify.EndTime.Hours, notify.EndTime.Minutes, notify.EndTime.Seconds);

                long millisecondsUntilStart = (long)startDateTime.Subtract(currentDate).TotalMilliseconds;
                long millisecondsUntilEnd = (long)endDateTime.Subtract(currentDate).TotalMilliseconds;
                if (millisecondsUntilStart > 0)
                {
                    _logger.LogInformation($"Notification: {notify.Title} Start: {millisecondsUntilStart}");
                    int startJsHandle = await _notificationApiInterop.ScheduleNotification((long)millisecondsUntilStart, "START: " + notify.Title, notify.Description, notify.ImgUrl);
                    _scheduledNotifications[notify.Id].Add(startJsHandle);
                }
                if (millisecondsUntilEnd > 0)
                {
                    _logger.LogInformation($"Notification: {notify.Title} End: {millisecondsUntilStart}");
                    int endJsHandle = await _notificationApiInterop.ScheduleNotification((long)millisecondsUntilEnd, "END: " + notify.Title, notify.Description, notify.ImgUrl);
                    _scheduledNotifications[notify.Id].Add(endJsHandle);
                }
                currentDate = currentDate.AddDays(1);
            }
        }

        public async Task DeRegisterNotification(Guid notifyId)
        {
            if (_scheduledNotifications.ContainsKey(notifyId))
            {
                foreach (var jsHandle in _scheduledNotifications[notifyId])
                {
                    await _notificationApiInterop.CancelNotification(jsHandle);
                }
            }
            _scheduledNotifications.Remove(notifyId);
        }
    }
}
