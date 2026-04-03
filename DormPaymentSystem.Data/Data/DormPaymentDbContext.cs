using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Seeders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Data
{
    public class DormPaymentDbContext : IdentityDbContext<User>
    {


        public DormPaymentDbContext(DbContextOptions<DormPaymentDbContext> options) : base(options)
        {

        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Floor> Floors { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Guest> Guests { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            RoleSeeder.SeedRoles(modelBuilder);

            modelBuilder.Entity<Payment>().
            HasOne(p => p.Student)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                 .HasOne(p => p.ReceivedBy)
                 .WithMany(u => u.ReceivedPayments)
                 .HasForeignKey(p => p.ReceivedById)
                 .OnDelete(DeleteBehavior.SetNull); // if user deleted, don't delete payment

            // ✅ Add - decimal precision
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }
    }
}