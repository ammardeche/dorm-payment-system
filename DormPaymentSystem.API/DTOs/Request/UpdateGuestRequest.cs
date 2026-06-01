using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class UpdateGuestRequest
    {
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
    }
}