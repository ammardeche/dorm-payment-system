using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAdmin(UserManager<User> _userManager)
        {

            string Email = "admin@dorm.com";

            var adminUser = await _userManager.FindByEmailAsync(Email);

            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = Email,
                    Email = Email,
                    FirstName = "Admin",
                    LastName = "User",
                };

                var result = await _userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }
    }
}