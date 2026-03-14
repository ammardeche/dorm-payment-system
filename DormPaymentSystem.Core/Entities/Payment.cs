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

        public PaymentStatus Status = PaymentStatus.Pending;

        public PaymentMethod? Method { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }


    }

}
