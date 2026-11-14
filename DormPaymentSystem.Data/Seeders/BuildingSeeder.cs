using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Seeders
{
    public class BuildingSeeder
    {
         public static async Task SeedBuildings(ModelBuilder modelBuilder)
        {

             modelBuilder.Entity<Building>().HasData(
            new Building { Id = 1, Code = "DS 1", Name = "Filon" },
            new Building { Id = 2, Code = "DS 2", Name = "Laura" },
            new Building { Id = 3, Code = "DS 3", Name = "Bartek" },
            new Building { Id = 4, Code = "DS 6", Name = "Mimoza" },
            new Building { Id = 5, Code = "DS 7", Name = "Proton" }
            );
        }
    }
}