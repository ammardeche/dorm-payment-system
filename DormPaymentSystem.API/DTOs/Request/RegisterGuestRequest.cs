
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class RegisterGuestRequest
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string DocumentNumber { get; set; } = null!;
        public DocumentType DocumentType { get; set; }
        [Required]
        public int BuildingId { get; set; }
    }
}