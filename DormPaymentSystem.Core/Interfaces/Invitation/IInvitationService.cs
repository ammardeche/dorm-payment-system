using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IInvitationService
    {

        Task<(IEnumerable<Invitation> Items, int TotalCount)> GetInvitationsAsync(
           int? studentId = null,
           int? guestId = null,
           int? month = null,
           int? year = null,
           int pageIndex = 1,
           int pageSize = 10);

        Task<Invitation?> GetInvitationByIdAsync(int id);

        Task<Invitation> CreateInvitationAsync(
            int invitedByStudentId,
            int guestId);            // guest record must exist first

        Task<bool> DeleteInvitationAsync(int id);

        Task<bool> CanStudentInviteThisMonthAsync(int studentId);

    }
}