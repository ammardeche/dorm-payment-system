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
            string? documentNumber = null,
            DocumentType? documentType = null,
            string? fullName = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _guestRepository.GetGuests(
                documentNumber, documentType, fullName, pageIndex, pageSize);
        }

        public async Task<Guest?> GetGuestByIdAsync(int id)
        {
            var guest = await _guestRepository.GetGuestById(id);
            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{id}' not found.");
            return guest;
        }

        public async Task<Guest?> GetGuestByDocumentNumberAsync(string documentNumber, DocumentType? documentType)
        {
            return await _guestRepository.GetGuestByDocumentNumber(documentNumber, documentType);
        }

        public async Task<Guest> RegisterGuestAsync(string fullName, string documentNumber, DocumentType? documentType)
        {
            // validate
            if (string.IsNullOrWhiteSpace(fullName))
                throw new AppValidationException("Full name cannot be empty.");

            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new AppValidationException("National ID cannot be empty.");


            // check duplicate national ID
            var existing = await _guestRepository.GetGuestByDocumentNumber(documentNumber, documentType);
            if (existing != null)
            {
                return existing;
            }

            var guest = new Guest
            {
                FullName = fullName,
                DocumentNumber = documentNumber,
                DocumentType = documentType,
            };

            return await _guestRepository.CreateGuest(guest);
        }

        public async Task<Guest> UpdateGuestAsync(int id, string? fullName, string? documentNumber, DocumentType? documentType)
        {
            var guest = await _guestRepository.GetGuestById(id);
            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{id}' not found.");

            if (!string.IsNullOrWhiteSpace(fullName)) guest.FullName = fullName;

            var newNumber = string.IsNullOrWhiteSpace(documentNumber)
            ? guest.DocumentNumber
            : documentNumber.Trim().ToUpperInvariant();
            var newType = documentType ?? guest.DocumentType;

            if (newNumber != guest.DocumentNumber && newType != guest.DocumentType)
            {
                var existing = await _guestRepository.GetGuestByDocumentNumber(newNumber, newType);
                if (existing != null && existing.Id != id)
                    throw new AppConflictException(
                        $"Document '{newNumber}' ({newType}) is already used by another guest.");

                guest.DocumentNumber = newNumber;
                guest.DocumentType = newType;
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



        // public Task<Guest> CheckOutGuestAsync(int reservationId)
        // {
        //    var guest = _guestRepository
        // }
    }

}

