using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class GuestRepository : IGuestRepository
    {

        private readonly DormPaymentDbContext _context;
        public GuestRepository(DormPaymentDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Guest>> GetGuests(
        string? nationalId = null,
        int? roomId = null,
        bool? isActive = null
        )
        {
            var query = _context.Guests
               .AsNoTracking()
               .Include(g => g.Room)
               .Include(g => g.Payments)
               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nationalId))
                query = query.Where(g => g.NationalId == nationalId);

            if (roomId.HasValue)
                query = query.Where(g => g.RoomId == roomId.Value);

            // active = still checked in = no checkout date
            if (isActive.HasValue)
                query = isActive.Value
                    ? query.Where(g => g.CheckOutDate == null)
                    : query.Where(g => g.CheckOutDate != null);

            return await query.ToListAsync();

        }

        public async Task<Guest> CreateGuest(Guest guest)
        {
            await _context.Guests.AddAsync(guest);
            await SaveChanges();
            return guest;
        }

        public async Task<Guest> UpdateGuest(Guest guest)
        {
            _context.Guests.Update(guest);
            await SaveChanges();
            return guest;
        }

        public async Task<bool> DeleteGuest(Guest guest)
        {
            _context.Guests.Remove(guest);
            await SaveChanges();
            return true;
        }

        public async Task<bool> GuestExists(int id)
        {
            return await _context.Guests.AnyAsync(g => g.Id == id);
        }

        public async Task<Guest?> GetGuestById(int id)
        {
            var guest = await _context.Guests.FindAsync(id);

            return guest;
        }


        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Guest?> CheckGuestByNationalIdAsync(string nationalId)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.NationalId == nationalId);
        }
    }
}


