using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class LoginResult
    {
        public User User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? StudentId { get; set; }    // only for students
    }
}