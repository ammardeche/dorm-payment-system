using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IGuestService
    {
        // READ
        Task<IEnumerable<Guest>> GetAllGuestsAsync();
        Task<Guest?> GetGuestByIdAsync(int id);
        Task<IEnumerable<Guest>> GetGuestsByRoomAsync(int roomId);

        // WRITE
        Task<Guest> CheckInGuestAsync();    // creates guest + snaps rate add a parameter to dto if needed
        Task<Guest> CheckOutGuestAsync(int guestId);          // calculates total, triggers payment

        // BUSINESS LOGIC
        Task<decimal> CalculateTotalAmountAsync(int guestId); // NightsStayed × RatePerNight
    }
}