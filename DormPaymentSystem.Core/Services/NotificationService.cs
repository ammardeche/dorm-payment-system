using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class NotificationService : INotificationService
    {

        private readonly INotificationRepository _notificationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRoomRepository _roomRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IStudentRepository studentRepository,
            IRoomRepository roomRepository)
        {
            _notificationRepository = notificationRepository;
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
        }

        public async Task<(IEnumerable<Notification> Items, int TotalCount)>
            GetAdminNotificationsAsync(
                string adminUserId,
                bool? isRead = null,
                int pageIndex = 1,
                int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _notificationRepository.GetNotifications(
                adminUserId, isRead, pageIndex, pageSize);
        }

        public async Task<(IEnumerable<Notification> Items, int TotalCount)>
            GetStudentNotificationsAsync(
                string studentUserId,
                bool? isRead = null,
                int pageIndex = 1,
                int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _notificationRepository.GetNotifications(
                studentUserId, isRead, pageIndex, pageSize);
        }

        public async Task SendToStudentAsync(
            string senderUserId,
            string recipientUserId,
            string title,
            string message,
            NotificationType type)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new AppValidationException("Title cannot be empty.");

            if (string.IsNullOrWhiteSpace(message))
                throw new AppValidationException("Message cannot be empty.");

            var notification = new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                RecipientUserId = recipientUserId,
                SenderUserId = senderUserId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepository.CreateNotification(notification);
        }

        public async Task SendToRoomAsync(
            string senderUserId,
            int roomId,
            string title,
            string message,
            NotificationType type)
        {
            // get all active students in the room
            var (students, _) = await _studentRepository
                .GetAllStudents(roomId: roomId, isActive: true);

            if (!students.Any())
                throw new AppNotFoundException(
                    $"No active students found in room '{roomId}'.");

            // one notification per student
            foreach (var student in students)
            {
                var notification = new Notification
                {
                    Title = title,
                    Message = message,
                    Type = type,
                    RecipientUserId = student.UserId,
                    SenderUserId = senderUserId,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                await _notificationRepository.CreateNotification(notification);
            }
        }

        public async Task SendToAllStudentsAsync(
            string senderUserId,
            string title,
            string message,
            NotificationType type)
        {
            // get all active students — large page size to get all
            var (students, _) = await _studentRepository
                .GetAllStudents(isActive: true, pageSize: 1000);

            if (!students.Any())
                throw new AppNotFoundException("No active students found.");

            foreach (var student in students)
            {
                var notification = new Notification
                {
                    Title = title,
                    Message = message,
                    Type = type,
                    RecipientUserId = student.UserId,
                    SenderUserId = senderUserId,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                await _notificationRepository.CreateNotification(notification);
            }
        }

        // called by background job — no sender, system generated
        public async Task SendToAdminAsync(
            string adminUserId,
            string title,
            string message,
            NotificationType type)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                RecipientUserId = adminUserId,
                SenderUserId = null,    // system generated
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepository.CreateNotification(notification);
        }

        public async Task MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _notificationRepository
                .GetNotificationById(notificationId);

            if (notification == null)
                throw new AppNotFoundException("Notification not found.");

            // user can only mark their own notifications as read
            if (notification.RecipientUserId != userId)
                throw new AppValidationException(
                    "You can only mark your own notifications as read.");

            if (notification.IsRead) return; // already read, do nothing

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            await _notificationRepository.UpdateNotification(notification);
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var (notifications, _) = await _notificationRepository
                .GetNotifications(userId, isRead: false, pageSize: 1000);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _notificationRepository.UpdateNotification(notification);
            }
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _notificationRepository.GetUnreadCount(userId);
        }
    }

}
