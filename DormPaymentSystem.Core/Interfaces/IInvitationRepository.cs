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


        Task<Invitation> CreateInvitation(Invitation invitation);
        Task<bool> DeleteInvitation(Invitation invitation);


        Task<bool> RoomHasInvitationThisMonth(int roomId, int month, int year);

        Task<Invitation?> GetInvitationById(int id);
        Task<Invitation> UpdateInvitation(Invitation invitation);



    }
}