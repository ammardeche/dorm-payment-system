using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IDormitorySettingsRepository
    {
        Task<DormitorySettings?> GetCurrentSettings();

        Task<DormitorySettings> CreateSettings(
            DormitorySettings settings);

        Task<DormitorySettings> UpdateSettings(
            DormitorySettings settings);

        Task SaveChanges();
    }
}