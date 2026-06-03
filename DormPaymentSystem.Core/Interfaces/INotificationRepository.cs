using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Interfaces
{
    public interface INotificationRepository
    {
        Task<(IEnumerable<Notification> Items, int TotalCount)> GetNotifications(
            string recipientUserId,
            bool? isRead = null,
            int pageIndex = 1,
            int pageSize = 10);

        Task<Notification?> GetNotificationById(int id);
        Task<int> GetUnreadCount(string recipientUserId);

        Task<Notification> CreateNotification(Notification notification);
        Task<Notification> UpdateNotification(Notification notification);
        Task SaveChanges();
    }
}