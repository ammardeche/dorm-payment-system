using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class UpdatePaymentRequest
    {
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public string? Note { get; set; }
    }
}