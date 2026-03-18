using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Data
{
    public class DormPaymentDbContext : IdentityDbContext
    {


        public DormPaymentDbContext(DbContextOptions<DormPaymentDbContext> options) : base(options)
        {

        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Floor> Floors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}