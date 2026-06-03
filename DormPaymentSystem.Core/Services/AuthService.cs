using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class AuthService : IAuthService
    {


        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IStudentRepository _studentRepository;

        public AuthService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _studentRepository = studentRepository;
        }

        // admin and receptionist login with email
        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Password is required.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new AppNotFoundException("Invalid email or password.");

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
                throw new UnauthorizedException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            // block students from using this endpoint
            if (roles.Contains("Student"))
                throw new UnauthorizedException(
                    "Students must use the student login endpoint.");

            var token = _tokenService.GenerateToken(user, roles);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new LoginResult
            {
                User = user,
                Token = token,
                Role = roles.FirstOrDefault()!,
                StudentId = null
            };
        }

        // student logs in with studentNumber + password
        public async Task<LoginResult> StudentLoginAsync(
            string studentNumber, string password)
        {
            if (string.IsNullOrWhiteSpace(studentNumber))
                throw new AppValidationException("Student number is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Password is required.");

            // find by username (which is the studentNumber)
            var user = await _userManager.FindByNameAsync(studentNumber);
            if (user == null)
                throw new AppNotFoundException("Invalid student number or password.");

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
                throw new UnauthorizedException("Invalid student number or password.");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Student"))
                throw new UnauthorizedException("This account is not a student account.");

            // get the student record to embed studentId in token
            var student = await _studentRepository.GetStudentByUserId(user.Id);
            if (student == null)
                throw new AppNotFoundException("Student profile not found.");

            if (!student.IsActive)
                throw new UnauthorizedException("Your account has been deactivated. Contact the manager.");

            // token includes studentId claim
            var token = _tokenService.GenerateToken(user, roles, student.Id);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new LoginResult
            {
                User = user,
                Token = token,
                Role = "Student",
                StudentId = student.Id
            };
        }

        // admin registers staff (Admin or Receptionist)
        public async Task<User> RegisterStaffAsync(
            string firstName,
            string lastName,
            string email,
            string password,
            string role)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new AppValidationException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new AppValidationException("Last name is required.");
            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Password is required.");

            // only Admin and Receptionist allowed here
            if (role != "Admin" && role != "Receptionist")
                throw new AppValidationException(
                    "Role must be Admin or Receptionist.");

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
                throw new AppConflictException("Email already registered.");

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new AppValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role);
            return user;
        }

        // manager creates student login account
        // called by StudentController.CreateStudent
        public async Task<string> RegisterStudentAccountAsync(
            string studentNumber,
            string password,
            string firstName,
            string lastName)
        {
            if (string.IsNullOrWhiteSpace(studentNumber))
                throw new AppValidationException("Student number is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new AppValidationException("Password is required.");

            // username = studentNumber — this is how student logs in
            var existing = await _userManager.FindByNameAsync(studentNumber);
            if (existing != null)
                throw new AppConflictException(
                    $"Account for student number '{studentNumber}' already exists.");

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = studentNumber,   // login with studentNumber
                Email = null               // email optional for students
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new AppValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Student");

            // return UserId so StudentService can link Student → User
            return user.Id;
        }

        public async Task ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new AppNotFoundException("User not found.");

            var result = await _userManager.ChangePasswordAsync(
                user, currentPassword, newPassword);

            if (!result.Succeeded)
                throw new AppValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task UpdateFcmTokenAsync(string userId, string fcmToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new AppNotFoundException("User not found.");

            user.FcmDeviceToken = fcmToken;
            user.FcmTokenUpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }


    }

}
