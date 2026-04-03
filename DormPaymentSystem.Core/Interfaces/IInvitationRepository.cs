using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IInvitationRepository
    {
        // READ
        Task<IEnumerable<Invitation>> GetAllInvitations();
        Task<Invitation?> GetInvitationById(int id);
        Task<IEnumerable<Invitation>> GetInvitationsByRoom(int roomId);
        Task<IEnumerable<Invitation>> GetInvitationsByStudent(int studentId);

        // WRITE
        Task<Invitation> CreateInvitation(Invitation invitation);
        Task<bool> DeleteInvitation(int id);

        // CHECK — the key rule
        Task<bool> RoomHasInvitationThisMonth(int roomId, int month, int year);

        Task SaveChanges();
    }
}