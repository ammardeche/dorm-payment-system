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
        private readonly IRoomRepository _roomRepository;


        public GuestService(IGuestRepository guestRepository,
        IRoomRepository roomRepository
    )
        {

            _guestRepository = guestRepository;
            _roomRepository = roomRepository;

        }

        public async Task<Guest?> CheckGuestByNationalIdAsync(string nationalId)
        {
            var guest = await _guestRepository.CheckGuestByNationalIdAsync(nationalId);
            return guest;
        }

        public async Task<Guest> CheckInGuestAsync(
        string fullName,
        string nationalId,
        int roomId,
        decimal ratePerNight,
        int? nightsStayed = null
        )
        {
            var existingGuest = await _guestRepository.CheckGuestByNationalIdAsync(nationalId);
            if (existingGuest != null)
            {
                throw new AppConflictException($"{fullName} already exists.");
            }
            // 1. validate cheap things first
            if (string.IsNullOrWhiteSpace(fullName))
                throw new AppValidationException("Full name cannot be empty.");

            if (string.IsNullOrWhiteSpace(nationalId))
                throw new AppValidationException("National ID cannot be empty.");

            if (ratePerNight <= 0)
                throw new AppValidationException("Rate per night must be greater than 0.");

            // if you get the value of stayed nights 
            var nights = nightsStayed ?? 0;
            var total = nights * ratePerNight;

            // 2. check room exists
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            // 3. check room is not full
            var activeStudents = room.Students.Count(s => s.IsActive);
            if (activeStudents >= room.Capacity)
                throw new AppConflictException("Room is already full.");

            // 4. create guest
            var guest = new Guest
            {
                FullName = fullName,
                NationalId = nationalId,
                RoomId = roomId,
                RatePerNight = ratePerNight,
                CheckInDate = DateTime.UtcNow,
                CheckOutDate = null,
                NightsStayed = nights,
                TotalAmount = total,
            };



            return await _guestRepository.CreateGuest(guest);
        }

        public async Task<Guest> CheckOutGuestAsync(int guestId)
        {
            var guest = await _guestRepository.GetGuestById(guestId);

            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{guestId}' not found.");

            // check not already checked out
            if (guest.CheckOutDate != null)
                throw new AppValidationException("Guest has already checked out.");

            // set checkout date
            guest.CheckOutDate = DateTime.UtcNow;

            // if nights were not set on check in — calculate now
            if (guest.NightsStayed == 0)
            {
                // calculate nights stayed
                var nights = (int)Math.Ceiling(
                    (guest.CheckOutDate.Value - guest.CheckInDate).TotalDays);

                // at least 1 night
                guest.NightsStayed = nights == 0 ? 1 : nights;

                // calculate total using stored rate
                guest.TotalAmount = guest.NightsStayed * guest.RatePerNight;
            }

            return await _guestRepository.UpdateGuest(guest);
        }

        public async Task<IEnumerable<Guest>> GetAllGuestsAsync(int? roomId = null, string? nationalId = null, bool? isActive = null)
        {
            var guests = await _guestRepository.GetGuests(
                roomId: roomId,
                nationalId: nationalId,
                isActive: isActive
            );
            return guests;
        }

        public async Task<Guest?> GetGuestByIdAsync(int id)
        {
            var guest = await _guestRepository.GetGuestById(id);

            if (guest == null)
            {
                throw new AppNotFoundException("guest doesn't exist");
            }

            return guest;
        }
    }

}