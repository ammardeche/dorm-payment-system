using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // who sent it — null means system generated
        public NotificationSenderResponse? Sender { get; set; }

        public NotificationResponse(Notification n)
        {
            Id = n.Id;
            Title = n.Title;
            Message = n.Message;
            Type = n.Type;
            IsRead = n.IsRead;
            CreatedAt = n.CreatedAt;
            ReadAt = n.ReadAt;

            Sender = n.Sender != null
                ? new NotificationSenderResponse
                {
                    Id = n.Sender.Id,
                    // system generated shows "System" as name
                    Name = n.SenderUserId == null
                        ? "System"
                        : n.Sender.UserName ?? "Unknown"
                }
                : null;
        }
    }

    public class NotificationSenderResponse
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

}
