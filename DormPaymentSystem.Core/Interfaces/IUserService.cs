using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllReceptionists();
        Task<User> GetUserById(string id);
        Task<User> UpdateUser(string id, string firstName, string lastName, string email);
        Task DeleteUser(string id);
    }
}
