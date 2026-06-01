using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class RegisterGuestRequest
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string NationalId { get; set; } = null!;
    }
}