using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public ReservationType Type { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;

        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        // copied from DormitorySettings at booking time
        // never changes even if admin updates prices later
        public decimal RateSnapshot { get; set; }

        // StudentStay   → StudentMonthlyRate x months
        // GuestStay     → GuestRatePerNight x nights
        // InvitationStay → always 0
        public decimal TotalAmount { get; set; }

        // always set — every stay is in a room
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        // only ONE of these three is set depending on Type
        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        public int? GuestId { get; set; }
        public Guest? Guest { get; set; }

        public int? InvitationId { get; set; }
        public Invitation? Invitation { get; set; }

        // navigation
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}