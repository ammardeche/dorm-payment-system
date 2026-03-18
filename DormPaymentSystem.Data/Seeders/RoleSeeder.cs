using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Seeders
{
    public class RoleSeeder
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "f86a91e0-a112-4bd3-8530-eafae265ba95", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "112d03d3-f355-4074-944e-3be9a22984ce", Name = "Receptionist", NormalizedName = "RECEPTIONIST" }
            );
        }
    }
}