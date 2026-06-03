using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class SendNotificationRequest
    {
        [Required] public string Title { get; set; } = null!;
        [Required] public string Message { get; set; } = null!;
        [Required] public NotificationType Type { get; set; }
    }
}