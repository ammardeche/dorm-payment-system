using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Interfaces;

namespace DormPaymentSystem.Core.Services
{
    public class NotificationService : INotificationService
    {

        private readonly INotificationRepository _notificationRepository;
        private readonly IStudentRepository _studentRepository;
        public NotificationService(
        INotificationRepository notificationRepository,
        IStudentRepository studentRepository
        )
        {
            _notificationRepository = notificationRepository;
            _studentRepository = studentRepository;
        }
        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetUnreadNotifications();
            return notifications;
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            await _notificationRepository.MarkAsRead(notificationId);
        }

        // check it for tomorrow
        public async Task SendDailyPaymentRemindersAsync()
        {

            // 1. Get all active students
            var students = await _studentRepository.GetAllStudents(null, true, null);

            // 2. Determine the current month (first day of month)
            var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            // 3. Admin user who receives notifications
            var adminId = "d9e71e86-a983-4ae9-a5c9-0ae39b056b7f"; // replace with real admin ID

            foreach (var student in students)
            {
                // 4. Start from the student's enrollment month
                var month = new DateTime(student.EnrollmentDate.Year, student.EnrollmentDate.Month, 1);

                var unpaidMonths = new List<string>();

                // 5. Loop through each month until current month
                while (month <= currentMonth)
                {
                    bool paid = student.Payments.Any(p =>
                        p.PaymentYear == month.Year &&
                        p.PaymentMonth == month.Month &&
                        p.Status == PaymentStatus.Paid);

                    if (!paid)
                    {
                        unpaidMonths.Add(month.ToString("MMMM yyyy"));
                    }

                    month = month.AddMonths(1);
                }

                // 6. If student has unpaid months → create ONE notification
                if (unpaidMonths.Any())
                {
                    var message =
                        $"{student.FirstName} {student.LastName} has unpaid months: {string.Join(", ", unpaidMonths)}.";

                    var notification = new Notification
                    {
                        Title = "Outstanding Payments",
                        Message = message,
                        Type = NotificationType.overDue,
                        UserId = adminId,
                        StudentId = student.Id,
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    await _notificationRepository.CreateNotification(notification);
                }
            }
        }
    }
}