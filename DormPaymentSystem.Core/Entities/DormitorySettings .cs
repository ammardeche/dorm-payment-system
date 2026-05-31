using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class DormitorySettings
    {
        public int Id { get; set; }

        public decimal GuestRatePerNight { get; set; }

        public decimal StudentMonthlyRate { get; set; }

        // track when this pricing became active
        public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}