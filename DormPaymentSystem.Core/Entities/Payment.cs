using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ReceiptNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentMethod? Method { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public int DaysLate { get; set; }

        // who recorded this payment (admin or receptionist)
        public string? ReceivedById { get; set; }
        public User? ReceivedBy { get; set; }

        // soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedById { get; set; }

        // replaces old StudentId + GuestId
        // to know who paid → payment.Reservation.Student or payment.Reservation.Guest
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;

    }

}
