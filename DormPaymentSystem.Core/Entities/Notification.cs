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

        public bool IsRead { get; set; } = false;

        public NotificationType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // who receives it
        public string UserId { get; set; } = null!;
        public User? User { get; set; }

        // which student it's about
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
    }
}