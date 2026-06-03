using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DormPaymentSystem.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        // mobile push notifications
        public string? FcmDeviceToken { get; set; }
        public DateTime? FcmTokenUpdatedAt { get; set; }

        // navigation
        public Student? Student { get; set; }
        public ICollection<Payment> ReceivedPayments { get; set; } = new List<Payment>();
        public ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
        public ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    }
}