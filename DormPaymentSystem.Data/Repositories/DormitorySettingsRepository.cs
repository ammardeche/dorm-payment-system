using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class DormitorySettingsRepository : IDormitorySettingsRepository
    {
        private readonly DormPaymentDbContext _context;

        public DormitorySettingsRepository(
            DormPaymentDbContext context)
        {
            _context = context;
        }

        public async Task<DormitorySettings?> GetCurrentSettings()
        {
            return await _context.DormitorySettings
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<DormitorySettings> CreateSettings(
            DormitorySettings settings)
        {
            await _context.DormitorySettings.AddAsync(settings);
            await SaveChanges();

            return settings;
        }

        public async Task<DormitorySettings> UpdateSettings(
            DormitorySettings settings)
        {
            _context.DormitorySettings.Update(settings);
            await SaveChanges();

            return settings;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
