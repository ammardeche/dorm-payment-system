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

        public string ReceiptNumber { get; set; } = null!;    // Required


        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public PaymentMethod? Method { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // navigation and foreign key for the who received the payment

        public string? ReceivedById { get; set; }
        public User? ReceivedBy { get; set; }

        // track which month this payment for 

        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }

        // days late if student pay after due date  

        public int DaysLate { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }


    }

}
