using System;

namespace Tomi.Notification.Core
{
    public interface INotify
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ImgUrl { get; set; }
    }
}
