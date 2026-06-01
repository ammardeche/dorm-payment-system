using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;

        // no longer needs IRoomRepository — room is handled by ReservationService
        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<(IEnumerable<Guest> Items, int TotalCount)> GetAllGuestsAsync(
            string? nationalId = null,
            string? fullName = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _guestRepository.GetGuests(
                nationalId, fullName, pageIndex, pageSize);
        }

        public async Task<Guest?> GetGuestByIdAsync(int id)
        {
            var guest = await _guestRepository.GetGuestById(id);
            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{id}' not found.");
            return guest;
        }

        public async Task<Guest?> GetGuestByNationalIdAsync(string nationalId)
        {
            return await _guestRepository.GetGuestByNationalId(nationalId);
        }

        public async Task<Guest> RegisterGuestAsync(string fullName, string nationalId)
        {
            // validate
            if (string.IsNullOrWhiteSpace(fullName))
                throw new AppValidationException("Full name cannot be empty.");

            if (string.IsNullOrWhiteSpace(nationalId))
                throw new AppValidationException("National ID cannot be empty.");

            // check duplicate national ID
            var existing = await _guestRepository.GetGuestByNationalId(nationalId);
            if (existing != null)
                throw new AppConflictException(
                    $"A guest with national ID '{nationalId}' already exists.");

            var guest = new Guest
            {
                FullName = fullName,
                NationalId = nationalId
            };

            return await _guestRepository.CreateGuest(guest);
        }

        public async Task<Guest> UpdateGuestAsync(int id, string? fullName, string? nationalId)
        {
            var guest = await _guestRepository.GetGuestById(id);
            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{id}' not found.");

            if (!string.IsNullOrWhiteSpace(fullName)) guest.FullName = fullName;

            if (!string.IsNullOrWhiteSpace(nationalId))
            {
                // make sure new nationalId is not taken by another guest
                var existing = await _guestRepository.GetGuestByNationalId(nationalId);
                if (existing != null && existing.Id != id)
                    throw new AppConflictException(
                        $"National ID '{nationalId}' is already used by another guest.");

                guest.NationalId = nationalId;
            }

            return await _guestRepository.UpdateGuest(guest);
        }

        public async Task<bool> DeleteGuestAsync(int id)
        {
            var guest = await _guestRepository.GetGuestById(id);
            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{id}' not found.");

            // block delete if guest has active reservations
            var hasActiveReservation = guest.Reservations
                .Any(r => r.Status == Core.Enums.ReservationStatus.Active);

            if (hasActiveReservation)
                throw new AppValidationException(
                    "Cannot delete a guest with an active reservation.");

            await _guestRepository.DeleteGuest(guest);
            return true;
        }
    }

}

