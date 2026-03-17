using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DormPaymentSystem.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Role { get; set; } = null!;
        public DateTime? LastLogin { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}