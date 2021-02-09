using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Server.Data;

namespace Tomi.Calendar.Mono.Server.Services.Notification
{
    public class UserCalendarItemsNotificationItemsProvider
    {
        private readonly AppNpgSqlDataContext _appNpgSqlDataContext;

        public UserCalendarItemsNotificationItemsProvider(AppNpgSqlDataContext appNpgSqlDataContext)
        {
            _appNpgSqlDataContext = appNpgSqlDataContext;
        }

        private IEnumerable<UserNotificationItem> GetNotificationItems()
        {
            List<UserNotificationItem> _userNotificationItems = new List<UserNotificationItem>();
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
                        UserName = userCalendarItems.Key,
                        Title = calendarItem.Title,
                        Description = calendarItem.Description,
                        IconUrl = "https://i.imgur.com/LU8e5sj.jpg",
                        NotificationDateTime = DateTime.Today.AddHours(calendarItem.StartTime.Hour).AddMinutes(calendarItem.StartTime.Minute)
                    });

                    _userNotificationItems.Add(new UserNotificationItem()
                    {
                        UserName = userCalendarItems.Key,
                        Title = calendarItem.Title,
                        Description = calendarItem.Description,
                        IconUrl = "https://i.imgur.com/LU8e5sj.jpg",
                        NotificationDateTime = DateTime.Today.AddHours(calendarItem.EndTime.Hour).AddMinutes(calendarItem.EndTime.Minute)
                    });
                }
            }
            return _userNotificationItems;
        }

        public IEnumerable<UserNotificationItem> GetNotificationItems(DateTime notificationDateTime)
        {
            return GetNotificationItems().Where(n => n.NotificationDateTime == notificationDateTime).ToList();
        }
    }

    public class UserNotificationItem
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public DateTime NotificationDateTime { get; set; }
    }
}
