using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateDormitorySettingsRequest
    {

        public decimal GuestRatePerNight { get; set; }

        public decimal StudentMonthlyRate { get; set; }

        public DateTime EffectiveFrom { get; set; }

    }
}