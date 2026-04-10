using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class AppUserResponse
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? LastLogin { get; set; }

        public AppUserResponse(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            FullName = user.FullName;
            Email = user.Email!;
            LastLogin = user.LastLogin;
        }
    }
}
