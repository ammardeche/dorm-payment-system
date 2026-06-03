using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IInvitationRepository
    {
        Task<(IEnumerable<Invitation> Items, int TotalCount)> GetInvitations(
           int? studentId = null,
           int? guestId = null,
           int? month = null,
           int? year = null,
           int pageIndex = 1,
           int pageSize = 10);

        Task<Invitation?> GetInvitationById(int id);

        Task<bool> StudentHasInvitationThisMonth(
            int studentId, int month, int year);

        Task<Invitation> CreateInvitation(Invitation invitation);
        Task<Invitation> UpdateInvitation(Invitation invitation);
        Task<bool> DeleteInvitation(Invitation invitation);
        Task SaveChanges();
    }
}