using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IInvitationRepository
    {

        // READ — one flexible method replaces GetAll, ByRoom, ByStudent, ById
        Task<IEnumerable<Invitation>> GetInvitations(
            int? id = null,
            int? roomId = null,
            int? studentId = null,
            int? month = null,
            int? year = null
        );

        // WRITE
        Task<Invitation> CreateInvitation(Invitation invitation);
        Task<bool> DeleteInvitation(int id);

        // CHECK — the key rule
        Task<bool> RoomHasInvitationThisMonth(int roomId, int month, int year);

    }
}