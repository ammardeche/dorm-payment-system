using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;

namespace DormPaymentSystem.Core.Services
{
    public class InvitationService : IInvitationService
    {

        private readonly IInvitationRepository _invitationRepository;

        public InvitationService(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;

        }
        public Task<bool> CanRoomInviteThisMonthAsync(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task<Invitation> CreateInvitationAsync(string guestName, string guestIdentityId, int roomId, int invitedByStudentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteInvitationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Invitation>> GetInvitationsAsync(int? id = null, int? roomId = null, int? studentId = null, int? month = null, int? year = null)
        {
            throw new NotImplementedException();
        }
    }
}