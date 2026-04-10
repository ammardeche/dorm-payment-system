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
        Task<IEnumerable<Guest>> GetAllGuestsAsync(
        int? roomId = null,
        string? nationalId = null,
        bool? isActive = null);

        Task<Guest?> GetGuestByIdAsync(int id);

        // WRITE
        Task<Guest> CheckInGuestAsync(
            string fullName,
            string nationalId,
            int roomId,
            decimal ratePerNight,
           int? nightsStayed = null

            );    // receptionist enters price from form

        Task<Guest> CheckOutGuestAsync(int guestId);  // calculates total on checkout

        Task<Guest?> CheckGuestByNationalIdAsync(string nationalId);
    }
}