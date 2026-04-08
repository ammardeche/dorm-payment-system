using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;

namespace DormPaymentSystem.Core.Services
{
    public class GuestService : IGuestService
    {
        public Task<decimal> CalculateTotalAmountAsync(int guestId)
        {
            throw new NotImplementedException();
        }

        public Task<Guest> CheckInGuestAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Guest> CheckOutGuestAsync(int guestId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guest>> GetAllGuestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Guest?> GetGuestByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guest>> GetGuestsByRoomAsync(int roomId)
        {
            throw new NotImplementedException();
        }
    }
}