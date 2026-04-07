using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.Queries
{
    public class PaymentQuery
    {
        public int? StudentId { get; set; }
        public int? GuestId { get; set; }
        public PaymentStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ReceivedById { get; set; }
        public bool? DueToday { get; set; }
    }
}