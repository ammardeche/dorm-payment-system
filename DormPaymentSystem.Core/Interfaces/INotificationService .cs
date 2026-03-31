using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface INotificationService
    {
        Task SendDailyPaymentRemindersAsync();          // runs every morning at 8
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync();
        Task MarkAsReadAsync(int notificationId);
    }
}