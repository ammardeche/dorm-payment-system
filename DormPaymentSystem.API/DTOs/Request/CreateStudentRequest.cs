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
        [Required] public string FirstName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        [Required] public string Email { get; set; } = null!;
        [Required] public string StudentNumber { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int? RoomId { get; set; }
        [Required] public string Password { get; set; } = null!; // manager sets initial password
        // NO UserId here — generated internally

    }
}