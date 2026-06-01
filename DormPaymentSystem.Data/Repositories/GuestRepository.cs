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

        public async Task<(IEnumerable<Guest> Items, int TotalCount)> GetGuests(
            string? nationalId = null,
            string? fullName = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Guests
                .AsNoTracking()
                // load reservations so we can see stay history
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Room)
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Payments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nationalId))
                query = query.Where(g => g.NationalId == nationalId);

            if (!string.IsNullOrWhiteSpace(fullName))
                query = query.Where(g => g.FullName.Contains(fullName));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(g => g.FullName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Guest?> GetGuestById(int id)
        {
            return await _context.Guests
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Room)
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Payments)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Guest?> GetGuestByNationalId(string nationalId)
        {
            return await _context.Guests
                .FirstOrDefaultAsync(g => g.NationalId == nationalId);
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

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}


