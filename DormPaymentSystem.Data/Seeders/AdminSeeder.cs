using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAdmin(UserManager<User> _userManager , DormPaymentDbContext _context)
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

            foreach(var buildingId in new[] {1,2,3,4,5})
            {
                var exist = await _context.BuildingMemberships.AnyAsync(
                    m=>m.UserId == adminUser.Id && m.BuildingId == buildingId
                );

                if(!exist)
                {
                    
                    _context.BuildingMemberships.Add(
                        new BuildingMembership
                        {
                            UserId = adminUser.Id,
                            BuildingId = buildingId
                        }
                    );

                }
                await _context.SaveChangesAsync();
            
            }

        }
    }
}