using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IGuestRepository
    {
        // READ
        // READ (with filters)
        Task<IEnumerable<Guest>> GetGuests(
            string? nationalId = null,
            int? roomId = null,
            bool? isActive = null
        );
        Task<Guest?> GetGuestById(int id);

        // WRITE
        Task<Guest> CreateGuest(Guest guest);
        Task<Guest> UpdateGuest(Guest guest);
        Task<bool> DeleteGuest(Guest guest);

        // CHECK
        Task<bool> GuestExists(int id);

        Task<Guest?> CheckGuestByNationalIdAsync(string nationalId);


    }
}