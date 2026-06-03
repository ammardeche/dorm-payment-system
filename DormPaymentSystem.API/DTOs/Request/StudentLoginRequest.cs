using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class StudentLoginRequest
    {
        [Required] public string StudentNumber { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}