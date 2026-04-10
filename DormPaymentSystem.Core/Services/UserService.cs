using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class UserService : IUserService
    {


        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllReceptionists()
        {
            var receptionists = await _userManager.GetUsersInRoleAsync("Receptionist");
            return receptionists;
        }

        public async Task<User> GetUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new AppValidationException("User ID is required");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new AppNotFoundException($"User with id '{id}' not found");

            return user;
        }

        public async Task<User> UpdateUser(string id, string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new AppValidationException("User ID is required");
            if (string.IsNullOrWhiteSpace(firstName))
                throw new AppValidationException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new AppValidationException("Last name is required");
            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email is required");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new AppNotFoundException($"User with id '{id}' not found");

            if (user.Email != email)
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                    throw new AppConflictException("Email is already taken by another user");
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.UserName = email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AppValidationException(errors);
            }

            return user;
        }

        public async Task DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new AppValidationException("User ID is required");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new AppNotFoundException($"User with id '{id}' not found");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                throw new ForbiddenException("Cannot delete an admin user");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AppValidationException(errors);
            }
        }
    }

}
