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
        Task<IEnumerable<Guest>> GetAllGuests();
        Task<Guest?> GetGuestById(int id);
        Task<Guest?> GetGuestByNationalId(string nationalId);
        Task<IEnumerable<Guest>> GetGuestsByRoom(int roomId);
        Task<IEnumerable<Guest>> GetActiveGuests();

        // WRITE
        Task<Guest> CreateGuest(Guest guest);
        Task<Guest> UpdateGuest(Guest guest);
        Task<bool> DeleteGuest(int id);

        // CHECK
        Task<bool> GuestExists(int id);

        Task SaveChanges();
    }
}