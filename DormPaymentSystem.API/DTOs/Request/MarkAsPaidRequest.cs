using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class MarkAsPaidRequest
    {
        [Required] public PaymentMethod Method { get; set; }

    }
}