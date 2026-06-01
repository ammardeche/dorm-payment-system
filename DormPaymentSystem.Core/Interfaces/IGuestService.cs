using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IGuestService
    {


        Task<(IEnumerable<Guest> Items, int TotalCount)> GetAllGuestsAsync(
                  string? nationalId = null,
                  string? fullName = null,
                  int pageIndex = 1,
                  int pageSize = 10);

        Task<Guest?> GetGuestByIdAsync(int id);
        Task<Guest?> GetGuestByNationalIdAsync(string nationalId);

        // just registers guest identity — room/dates/payment handled by ReservationService
        Task<Guest> RegisterGuestAsync(string fullName, string nationalId);

        Task<Guest> UpdateGuestAsync(int id, string? fullName, string? nationalId);
        Task<bool> DeleteGuestAsync(int id);

    }
}