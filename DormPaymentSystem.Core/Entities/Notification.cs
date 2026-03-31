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

        public string? Title { get; set; }

        public bool IsRead = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public NotificationType Type { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}