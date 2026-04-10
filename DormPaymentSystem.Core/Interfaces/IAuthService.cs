using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IAuthService
    {

        Task<User> Register(string FirstName, string LastName, string Email, string Password, string ConfirmPassword, string Role);
        Task<LoginResult> Login(string Email, string Password);

    }
}