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

        public DormitorySettingsRepository(DormPaymentDbContext context)
        {
            _context = context;
        }

        // always returns the latest settings row
        public async Task<DormitorySettings?> GetCurrentSettingsAsync()
        {
            return await _context.DormitorySettings
                .OrderByDescending(s => s.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<DormitorySettings> CreateSettingsAsync(DormitorySettings settings)
        {
            await _context.DormitorySettings.AddAsync(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<DormitorySettings> UpdateSettingsAsync(DormitorySettings settings)
        {
            _context.DormitorySettings.Update(settings);
            await _context.SaveChangesAsync();
            return settings;
        }
    }
}
