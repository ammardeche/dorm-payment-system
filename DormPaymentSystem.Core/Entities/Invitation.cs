using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Entities
{
    public class Invitation
    {
        public int Id { get; set; }

        // when — enforces 1 invitation per student per month
        public int InvitationMonth { get; set; }
        public int InvitationYear { get; set; }
        public DateTime InvitationDate { get; set; } = DateTime.UtcNow;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        // who sent it
        public int InvitedByStudentId { get; set; }
        public Student InvitedByStudent { get; set; } = null!;

        // who is invited — Guest record created by receptionist first
        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;

        // null until guest physically checks in
        // filled when Reservation(Type=InvitationStay) is created
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}