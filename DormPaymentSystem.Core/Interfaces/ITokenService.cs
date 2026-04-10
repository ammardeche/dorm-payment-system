using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, IList<string> roles);
    }
}