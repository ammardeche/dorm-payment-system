using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Entities
{
    public class Notification
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }

        // who receives this notification
        public string RecipientUserId { get; set; } = null!;
        public User Recipient { get; set; } = null!;

        // who sent it — null if system generated
        public string? SenderUserId { get; set; }
        public User? Sender { get; set; }
    }
}