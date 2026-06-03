using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class RegisterStaffRequest
    {
        [Required] public string FirstName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
        [Required] public string Role { get; set; } = null!; // "Admin" or "Receptionist"
    }
}