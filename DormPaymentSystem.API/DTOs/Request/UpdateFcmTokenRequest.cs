using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class UpdateFcmTokenRequest
    {
        [Required] public string FcmToken { get; set; } = null!;

    }
}