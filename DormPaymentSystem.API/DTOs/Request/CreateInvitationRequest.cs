using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateInvitationRequest
    {

        public string GuestName { get; set; }
        public string GuestIdentityId { get; set; }
        public int RoomId { get; set; }
        public int InvitedByStudentId { get; set; }

    }
}