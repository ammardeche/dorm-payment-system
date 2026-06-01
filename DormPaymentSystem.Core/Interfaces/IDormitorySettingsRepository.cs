using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IDormitorySettingsRepository
    {
        Task<DormitorySettings?> GetCurrentSettingsAsync();
        Task<DormitorySettings> CreateSettingsAsync(DormitorySettings settings);
        Task<DormitorySettings> UpdateSettingsAsync(DormitorySettings settings);
    }
}