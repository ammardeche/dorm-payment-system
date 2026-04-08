using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;

namespace DormPaymentSystem.Data.Repositories
{
    public class GuestRepository : IGuestRepository
    {

        private readonly DormPaymentDbContext _context;
        public GuestRepository(DormPaymentDbContext context)
        {
            _context = context;
        }
        public Task<Guest> CreateGuest(Guest guest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGuest(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Guest>> GetGuests(int? id = null, string? nationalId = null, int? roomId = null, bool? isActive = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GuestExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<Guest> UpdateGuest(Guest guest)
        {
            throw new NotImplementedException();
        }
    }
}