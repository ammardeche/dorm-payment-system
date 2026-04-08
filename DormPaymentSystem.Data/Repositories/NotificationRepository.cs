using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Data;
using DormPaymentSystem.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly DormPaymentDbContext _context;

        public NotificationRepository(DormPaymentDbContext context)
        {
            _context = context;
        }
        public async Task<Notification> CreateNotification(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await SaveChanges();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotifications()
        {
            var unreadNotifications = await _context.Notifications.Where(p => p.IsRead == false).ToListAsync();
            return unreadNotifications;
        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            notification.IsRead = true;
            await SaveChanges();

        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}