using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
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

        // get guests 
        public async Task<(IEnumerable<Guest> Items, int TotalCount)> GetGuests(
            string? documentNumber = null,
            DocumentType? documentType = null,
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

            if (!string.IsNullOrWhiteSpace(documentNumber))
            {
                var number = documentNumber.Trim().ToUpperInvariant();
                query = query.Where(g => g.DocumentNumber.StartsWith(number));
            }

            if (!string.IsNullOrWhiteSpace(fullName))
                query = query.Where(g => g.FullName.Contains(fullName));


            if (documentType.HasValue)
                query = query.Where(g => g.DocumentType == documentType.Value);


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(g => g.FullName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // get by id 
        public async Task<Guest?> GetGuestById(int id)
        {
            return await _context.Guests
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Room)
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Payments)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        // get by document Number
        public async Task<Guest?> GetGuestByDocumentNumber(string documentNumber, DocumentType documentType)
        {
            return await _context.Guests
                .FirstOrDefaultAsync(g => g.DocumentNumber == documentNumber && g.DocumentType == documentType);
        }

        // create guest
        public async Task<Guest> CreateGuest(Guest guest)
        {
            await _context.Guests.AddAsync(guest);
            await SaveChanges();
            return guest;
        }

        // update guest
        public async Task<Guest> UpdateGuest(Guest guest)
        {
            _context.Guests.Update(guest);
            await SaveChanges();
            return guest;
        }

        // delete guest
        public async Task<bool> DeleteGuest(Guest guest)
        {
            _context.Guests.Remove(guest);
            await SaveChanges();
            return true;
        }

        // check if the guest exist 
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




//  we need to fix the guest repository now we added two new properties DocumentNumber documentType 
/* 
 - first thing we need to return the guest with their information ( name , docNumber , Doctype )

 */