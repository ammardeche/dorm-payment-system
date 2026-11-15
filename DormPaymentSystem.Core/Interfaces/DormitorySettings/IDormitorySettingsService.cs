using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IDormitorySettingsService
    {
        Task<DormitorySettings> GetCurrentSettingsAsync();

        Task<DormitorySettings> CreateSettingsAsync(
            decimal guestRatePerNight,
            decimal studentMonthlyRate);

        Task<DormitorySettings> UpdateSettingsAsync(
            decimal guestRatePerNight,
            decimal studentMonthlyRate);
    }
}