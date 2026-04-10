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

        public async Task<Invitation> CreateInvitation(Invitation invitation)
        {
            await _context.Invitations.AddAsync(invitation);
            await SaveChanges();
            return invitation;
        }

        public async Task<bool> DeleteInvitation(Invitation invitation)
        {
            _context.Invitations.Remove(invitation);
            await SaveChanges();
            return true;
        }

        public async Task<IEnumerable<Invitation>> GetInvitations(int? id = null, int? roomId = null, int? studentId = null, int? month = null, int? year = null)
        {
            var invitations = _context.Invitations.AsNoTracking()
            .Include(x => x.InvitedByStudent)
            .Include(x => x.Room)
            .AsQueryable();

            if (id.HasValue)
                invitations = invitations.Where(i => i.Id == id.Value);
            if (roomId.HasValue)
                invitations = invitations.Where(i => i.RoomId == roomId.Value);
            if (studentId.HasValue)
                invitations = invitations.Where(i => i.InvitedByStudentId == studentId.Value);
            if (month.HasValue)
                invitations = invitations.Where(i => i.InvitationMonth == month.Value);
            if (year.HasValue)
                invitations = invitations.Where(i => i.InvitationYear == year.Value);

            return await invitations.ToListAsync();

        }

        public async Task<bool> RoomHasInvitationThisMonth(int roomId, int month, int year)
        {
            return await _context.Invitations.AnyAsync(i =>
                  i.RoomId == roomId &&
                  i.InvitationMonth == month &&
                  i.InvitationYear == year
              );

        }


        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}