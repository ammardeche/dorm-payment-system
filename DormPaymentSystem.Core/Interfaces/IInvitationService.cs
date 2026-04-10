using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IInvitationService
    {

        Task<IEnumerable<Invitation>> GetInvitationsAsync(
            int? id = null,
            int? roomId = null,
            int? studentId = null,
            int? month = null,
            int? year = null
        );

        // WRITE
        Task<Invitation> CreateInvitationAsync(
            string guestName,
            string guestIdentityId,
            int roomId,
            int invitedByStudentId
        );
        Task<bool> DeleteInvitationAsync(int id);

        // BUSINESS LOGIC — the core rule
        Task<bool> CanRoomInviteThisMonthAsync(int roomId);

    }
}