using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class StudentLoginResponse
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
        public int StudentId { get; set; }
        public string Role { get; set; } = null!;
    }
}