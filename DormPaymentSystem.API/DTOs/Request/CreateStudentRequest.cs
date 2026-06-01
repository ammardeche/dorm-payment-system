using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateStudentRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int? RoomId { get; set; }
        public string UserId { get; set; } = null!;  // NEW

    }
}