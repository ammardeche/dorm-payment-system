using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class Invitation
    {
        public int Id { get; set; }

        // invited guest info — simple
        public string GuestName { get; set; } = null!;
        public string GuestIdentityId { get; set; } = null!;  // national ID or student number

        // which room invited — rule is per room
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        // which student sent the invite
        public int InvitedByStudentId { get; set; }
        public Student InvitedByStudent { get; set; } = null!;

        // when
        public int InvitationMonth { get; set; }
        public int InvitationYear { get; set; }
        public DateTime InvitationDate { get; set; } = DateTime.UtcNow;
    }
}