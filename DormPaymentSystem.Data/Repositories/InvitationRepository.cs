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
    public class InvitationRepository : IInvitationRepository
    {
        private readonly DormPaymentDbContext _context;

        public InvitationRepository(DormPaymentDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Invitation> Items, int TotalCount)> GetInvitations(
            int? studentId = null,
            int? guestId = null,
            int? month = null,
            int? year = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Invitations
                .AsNoTracking()
                .Include(i => i.InvitedByStudent)
                .Include(i => i.Guest)
                .Include(i => i.Reservation)
                .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(i => i.InvitedByStudentId == studentId.Value);

            if (guestId.HasValue)
                query = query.Where(i => i.GuestId == guestId.Value);

            if (month.HasValue)
                query = query.Where(i => i.InvitationMonth == month.Value);

            if (year.HasValue)
                query = query.Where(i => i.InvitationYear == year.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.InvitationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Invitation?> GetInvitationById(int id)
        {
            return await _context.Invitations
                .Include(i => i.InvitedByStudent)
                .Include(i => i.Guest)
                .Include(i => i.Reservation)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        // rule: 1 invitation per student per month
        public async Task<bool> StudentHasInvitationThisMonth(
            int studentId, int month, int year)
        {
            return await _context.Invitations.AnyAsync(i =>
                i.InvitedByStudentId == studentId &&
                i.InvitationMonth == month &&
                i.InvitationYear == year);
        }

        public async Task<Invitation> CreateInvitation(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
            await SaveChanges();
            return invitation;
        }

        public async Task<Invitation> UpdateInvitation(Invitation invitation)
        {
            _context.Invitations.Update(invitation);
            await SaveChanges();
            return invitation;
        }

        public async Task<bool> DeleteInvitation(Invitation invitation)
        {
            _context.Invitations.Remove(invitation);
            await SaveChanges();
            return true;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

}
