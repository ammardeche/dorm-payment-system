using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IInvitationService
    {
        // READ
        Task<IEnumerable<Invitation>> GetAllInvitationsAsync();
        Task<IEnumerable<Invitation>> GetInvitationsByRoomAsync(int roomId);
        Task<IEnumerable<Invitation>> GetInvitationsByStudentAsync(int studentId);

        // WRITE
        Task<Invitation> CreateInvitationAsync(); // parameters can be added as needed, e.g., roomId, studentId, etc.
        Task<bool> DeleteInvitationAsync(int id);

        // BUSINESS LOGIC — the core rule
        Task<bool> CanRoomInviteThisMonthAsync(int roomId);  // 1 invite per room per month
    }
}