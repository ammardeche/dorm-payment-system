using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IAuthService
    {

        // admin + receptionist login — email + password
        Task<LoginResult> LoginAsync(string email, string password);

        // student login — studentNumber + password
        Task<LoginResult> StudentLoginAsync(string studentNumber, string password);

        // admin registers other admins or receptionists
        Task<User> RegisterStaffAsync(
            string firstName,
            string lastName,
            string email,
            string password,
            string role);           // "Admin" or "Receptionist"

        // manager creates student login account
        // returns UserId to link with Student entity
        Task<string> RegisterStudentAccountAsync(
            string studentNumber,
            string password,
            string firstName,
            string lastName);

        Task ChangePasswordAsync(
            string userId,
            string currentPassword,
            string newPassword);

        // mobile app registers FCM token after login
        Task UpdateFcmTokenAsync(string userId, string fcmToken);
    }



}
