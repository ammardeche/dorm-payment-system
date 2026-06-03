using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class DormitorySettingsResponse
    {
        public int Id { get; set; }

        public decimal GuestRatePerNight { get; set; }

        public decimal StudentMonthlyRate { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DormitorySettingsResponse(
            DormitorySettings settings)
        {
            Id = settings.Id;
            GuestRatePerNight = settings.GuestRatePerNight;
            StudentMonthlyRate = settings.StudentMonthlyRate;
            EffectiveFrom = settings.EffectiveFrom;
        }
    }
}