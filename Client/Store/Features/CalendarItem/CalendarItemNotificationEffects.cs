using Fluxor;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Blazor.Notification;
using Tomi.Blazor.Notification.Services;
using Tomi.Calendar.Mono.Client.Store.Features.CalendarItem;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Store.Features.Notification
{
    public class CalendarItemNotificationEffects
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<CalendarItemNotificationEffects> _logger;
        private readonly IState<CalendarState> _state;

        public CalendarItemNotificationEffects(
            NotificationService notificationService,
            ILogger<CalendarItemNotificationEffects> logger,
            IState<CalendarState> state)
        {
            _notificationService = notificationService;
            _logger = logger;
            _state = state;
        }


        [EffectMethod]
        public async Task HandleAsync(LoadCalendarItemsSuccessAction action, IDispatcher dispatcher)
        {
            await _notificationService.RegisterNotifications(action.CalendarItems.Select(n => n.ToNotificationItem()));
        }

        [EffectMethod]
        public async Task HandleAsync(CreateCalendarItemSuccessAction action, IDispatcher dispatcher)
        {
            await _notificationService.RegisterNotification(action.CalendarItem.ToNotificationItem());
        }

        [EffectMethod]
        public async Task HandleAsync(UpdateCalendarItemSuccessAction action, IDispatcher dispatcher)
        {
            await _notificationService.RegisterNotification(action.CalendarItem.ToNotificationItem());
        }

        [EffectMethod]
        public async Task HandleAsync(DeleteCalendarItemSuccessAction action, IDispatcher dispatcher)
        {
            await _notificationService.DeRegisterNotification(action.Id);
        }
    }

    public static class LocalTimeExtensions
    {
        public static NotificationItem ToNotificationItem(this CalendarItemDto calendarItem)
        {
            return new NotificationItem()
            {
                Id = calendarItem.Id,
                Title = calendarItem.Title,
                Description = calendarItem.Description,
                ImgUrl = "",
                StartDate = calendarItem.StartDate.ToDateTime(),
                EndDate = calendarItem.EndDate.ToDateTime(),
                StartTime = calendarItem.StartTime.ToTimeSpan(),
                EndTime = calendarItem.EndTime.ToTimeSpan()
            };
        }
        public static TimeSpan ToTimeSpan(this LocalTime? localTime)
        {
            LocalTime lt = localTime.GetValueOrDefault();
            if (lt != default(LocalTime))
            {
                TimeSpan ts = new TimeSpan(lt.Hour, lt.Minute, lt.Second);
                return ts;
            }
            return default(TimeSpan);
        }
        public static DateTime ToDateTime(this LocalDate? localDate)
        {
            LocalDate ld = localDate.GetValueOrDefault();
            if (ld != default(LocalDate))
            {
                DateTime dt = ld.ToDateTimeUnspecified();
                return dt;
            }
            return default(DateTime);
        }
    }
}
