using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface INotificationService
    {
        // admin reads their own notifications (system alerts)
        Task<(IEnumerable<Notification> Items, int TotalCount)> GetAdminNotificationsAsync(
            string adminUserId,
            bool? isRead = null,
            int pageIndex = 1,
            int pageSize = 10);

        // student reads their own notifications on mobile
        Task<(IEnumerable<Notification> Items, int TotalCount)> GetStudentNotificationsAsync(
            string studentUserId,
            bool? isRead = null,
            int pageIndex = 1,
            int pageSize = 10);

        // admin sends to one student
        Task SendToStudentAsync(
            string senderUserId,
            string recipientUserId,
            string title,
            string message,
            NotificationType type);

        // admin sends to all students in a room
        Task SendToRoomAsync(
            string senderUserId,
            int roomId,
            string title,
            string message,
            NotificationType type);

        // admin sends to all active students
        Task SendToAllStudentsAsync(
            string senderUserId,
            string title,
            string message,
            NotificationType type);

        // system sends to admin — called by background job
        Task SendToAdminAsync(
            string adminUserId,
            string title,
            string message,
            NotificationType type);

        Task MarkAsReadAsync(int notificationId, string userId);
        Task MarkAllAsReadAsync(string userId);

        Task<int> GetUnreadCountAsync(string userId);
    }
}