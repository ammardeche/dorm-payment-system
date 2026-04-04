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

        [Required]
        public string FirstName { get; set; } = null!;
        [Required]

        public string LastName { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string StudentNumber { get; set; } = null!;
        [Required]
        public DateTime EnrollmentDay { get; set; }
        [Required]
        public int RoomId { get; set; }

    }
}