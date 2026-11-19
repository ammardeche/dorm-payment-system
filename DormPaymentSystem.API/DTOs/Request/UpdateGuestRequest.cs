using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class UpdateGuestRequest
    {
        public string? FullName { get; set; }
        public string? DocumentNumber { get; set; }

        public DocumentType? DocumentType { get; set; }
    }
}