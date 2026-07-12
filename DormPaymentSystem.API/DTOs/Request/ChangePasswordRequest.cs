using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class ChangePasswordRequest
    {
        [Required] public string CurrentPassword { get; set; } = null!;
        [Required] public string NewPassword { get; set; } = null!;
    }
}

