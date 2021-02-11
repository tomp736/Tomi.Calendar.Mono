using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.DataServices;
using Tomi.Calendar.Mono.Shared.Entities;
using Tomi.Notification.AspNetCore;
using Tomi.Notification.Core;

namespace Tomi.Calendar.Mono.Server.Services.Notification
{
    public class NotificationProcessingServiceDataProvider : INotificationProcessingServiceDataProvider
    {
        private readonly AppNpgSqlDataContext _appNpgSqlDataContext;
        private readonly DbContextEvents _dbContextEvents;
        private List<UserNotificationItem> _userNotificationItems;
        private bool _userNotificationItemsRefreshRequired;

        public NotificationProcessingServiceDataProvider(AppNpgSqlDataContext appNpgSqlDataContext, DbContextEvents dbContextEvents)
        {
            _appNpgSqlDataContext = appNpgSqlDataContext;
            _dbContextEvents = dbContextEvents;
            _dbContextEvents.EntitiesChanged += EntitiesChanged;
            _userNotificationItems = new List<UserNotificationItem>();
            _userNotificationItemsRefreshRequired = true;
        }

        private void EntitiesChanged(IEnumerable<Type> entitiesChanged)
        {
            if (entitiesChanged.Contains(typeof(CalendarItem)))
            {
                _userNotificationItemsRefreshRequired = true;
            }
        }

        private void RefreshUserNotificationItems()
        {
            _userNotificationItems?.Clear();

            var userCalendarItemDictionary = _appNpgSqlDataContext.Users
                   .Include(user => user.UserCalendarItems)
                   .ThenInclude(uci => uci.CalendarItem)
                   .AsNoTracking()
                   .ToDictionary(n => n.Id, n => n.UserCalendarItems.Select(uci => uci.CalendarItem));


            DateTime utcNow = DateTime.Now;
            LocalDate today = LocalDate.FromDateTime(utcNow.Date);
            LocalTime now = new LocalTime(utcNow.Hour, utcNow.Minute);

            foreach (var userCalendarItems in userCalendarItemDictionary)
            {
                foreach (var calendarItem in userCalendarItems.Value)
                {
                    _userNotificationItems.Add(new UserNotificationItem()
                    {
                        NotificationType = NotificationType.Group,
                        NotifyTypeName = userCalendarItems.Key,
                        Title = calendarItem.Title,
                        Description = calendarItem.Description,
                        IconUrl = "https://i.imgur.com/LU8e5sj.jpg",
                        NotificationDateTime = DateTime.Today.AddHours(calendarItem.StartTime.Hour).AddMinutes(calendarItem.StartTime.Minute)
                    });

                    _userNotificationItems.Add(new UserNotificationItem()
                    {
                        NotificationType = NotificationType.Group,
                        NotifyTypeName = userCalendarItems.Key,
                        Title = calendarItem.Title,
                        Description = calendarItem.Description,
                        IconUrl = "https://i.imgur.com/LU8e5sj.jpg",
                        NotificationDateTime = DateTime.Today.AddHours(calendarItem.EndTime.Hour).AddMinutes(calendarItem.EndTime.Minute)
                    });
                }
            }
            _userNotificationItemsRefreshRequired = false;
        }

        public IEnumerable<INotificationHubItem> GetNotificationItems(DateTime notificationDateTime)
        {
            if (_userNotificationItems == null || _userNotificationItemsRefreshRequired)
                RefreshUserNotificationItems();

            return _userNotificationItems.Where(n => n.NotificationDateTime == notificationDateTime).ToList();
        }
    }

    public class UserNotificationItem : INotificationHubItem
    {
        public NotificationType NotificationType { get; set; } = NotificationType.Group;
        public string NotifyTypeName { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public DateTime NotificationDateTime { get; set; }
    }
}
