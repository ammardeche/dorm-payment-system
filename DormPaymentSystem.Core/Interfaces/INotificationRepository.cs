using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetUnreadNotifications();
        Task<Notification> CreateNotification(Notification notification);
        Task MarkAsRead(int notificationId);
        Task SaveChanges();
    }
}