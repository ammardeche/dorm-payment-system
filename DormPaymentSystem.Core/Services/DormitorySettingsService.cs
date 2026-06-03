using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class DormitorySettingsService : IDormitorySettingsService
    {
        private readonly IDormitorySettingsRepository _repository;

        public DormitorySettingsService(
            IDormitorySettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<DormitorySettings> GetCurrentSettingsAsync()
        {
            var settings =
                await _repository.GetCurrentSettings();

            if (settings == null)
                throw new AppNotFoundException(
                    "Dormitory settings not found.");

            return settings;
        }

        public async Task<DormitorySettings> CreateSettingsAsync(
            decimal guestRatePerNight,
            decimal studentMonthlyRate)
        {
            var settings = new DormitorySettings
            {
                GuestRatePerNight = guestRatePerNight,
                StudentMonthlyRate = studentMonthlyRate,
                EffectiveFrom = DateTime.UtcNow
            };

            return await _repository.CreateSettings(settings);
        }

        public async Task<DormitorySettings> UpdateSettingsAsync(
            decimal guestRatePerNight,
            decimal studentMonthlyRate)
        {
            var settings =
                await _repository.GetCurrentSettings();

            if (settings == null)
                throw new AppNotFoundException(
                    "Dormitory settings not found.");

            settings.GuestRatePerNight = guestRatePerNight;
            settings.StudentMonthlyRate = studentMonthlyRate;
            settings.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateSettings(settings);
        }
    }
}