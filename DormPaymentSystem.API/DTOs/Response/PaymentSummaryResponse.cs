using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class PaymentSummaryResponse
    {
        public decimal TotalPaid { get; set; }
        public decimal TotalPending { get; set; }
        public int PendingCount { get; set; }
        public int OverdueCount { get; set; }

        // NEW
        public decimal TotalExpected { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}