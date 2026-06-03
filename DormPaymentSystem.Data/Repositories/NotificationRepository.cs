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

        public async Task<(IEnumerable<Notification> Items, int TotalCount)> GetNotifications(
            string recipientUserId,
            bool? isRead = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Notifications
                .AsNoTracking()
                .Include(n => n.Sender)
                .Where(n => n.RecipientUserId == recipientUserId)
                .AsQueryable();

            if (isRead.HasValue)
                query = query.Where(n => n.IsRead == isRead.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Notification?> GetNotificationById(int id)
        {
            return await _context.Notifications
                .Include(n => n.Sender)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<int> GetUnreadCount(string recipientUserId)
        {
            return await _context.Notifications
                .CountAsync(n =>
                    n.RecipientUserId == recipientUserId &&
                    !n.IsRead);
        }

        public async Task<Notification> CreateNotification(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await SaveChanges();
            return notification;
        }

        public async Task<Notification> UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            await SaveChanges();
            return notification;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
