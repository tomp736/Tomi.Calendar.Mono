using System;

namespace Tomi.Notification.Core
{
    public class NotificationItem : INotify
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
    }
}
