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
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<User> userManager, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<User> Register(string firstName, string lastName, string email, string password, string confirmPassword, string role)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new AppValidationException("First name is required");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new AppValidationException("Last name is required");
            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email is required");
            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Password is required");
            if (string.IsNullOrWhiteSpace(role))
                throw new AppValidationException("Role is required");

            if (password != confirmPassword)
                throw new AppValidationException("Password and Confirm Password do not match");

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                throw new AppConflictException("Email is already registered");

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AppValidationException(errors);
            }

            var assignRole = await _userManager.AddToRoleAsync(user, role);
            if (!assignRole.Succeeded)
            {
                var errors = string.Join(", ", assignRole.Errors.Select(e => e.Description));
                throw new AppValidationException(errors);
            }

            return user;
        }

        public async Task<LoginResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email is required");
            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Password is required");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new AppNotFoundException("User with this email doesn't exist");

            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!checkPassword)
                throw new UnauthorizedException("Invalid password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new LoginResult
            {
                User = user,
                Token = token,
                Role = roles.FirstOrDefault()!
            };
        }


    }

}
