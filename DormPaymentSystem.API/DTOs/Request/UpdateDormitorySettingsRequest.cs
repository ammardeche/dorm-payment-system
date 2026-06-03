using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class UpdateDormitorySettingsRequest
    {
        [Required]
        public decimal GuestRatePerNight { get; set; }

        [Required]
        public decimal StudentMonthlyRate { get; set; }
    }
}