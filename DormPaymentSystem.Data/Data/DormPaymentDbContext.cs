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

        public DbSet<Student> Students { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<DormitorySettings> DormitorySettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            RoleSeeder.SeedRoles(modelBuilder);

            // ─────────────────────────────────────────
            // STUDENT
            // ─────────────────────────────────────────

            // Student → User (one to one)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // one UserId per student
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();

            // one StudentNumber per student
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentNumber)
                .IsUnique();

            // Student → Room
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Students)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // ─────────────────────────────────────────
            // RESERVATION
            // ─────────────────────────────────────────

            // Reservation → Room
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation → Student (nullable)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation → Guest (nullable)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reservation → Invitation (nullable, one to one)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Invitation)
                .WithOne(i => i.Reservation)
                .HasForeignKey<Reservation>(r => r.InvitationId)
                .OnDelete(DeleteBehavior.SetNull);

            // decimal precision
            modelBuilder.Entity<Reservation>()
                .Property(r => r.RateSnapshot)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalAmount)
                .HasPrecision(18, 2);

            // ─────────────────────────────────────────
            // PAYMENT
            // ─────────────────────────────────────────

            // Payment → Reservation
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment → ReceivedBy (admin/receptionist)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.ReceivedBy)
                .WithMany(u => u.ReceivedPayments)
                .HasForeignKey(p => p.ReceivedById)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // ─────────────────────────────────────────
            // INVITATION
            // ─────────────────────────────────────────

            // Invitation → Student (who sent it)
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.InvitedByStudent)
                .WithMany(s => s.Invitations)
                .HasForeignKey(i => i.InvitedByStudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invitation → Guest (who is invited)
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Guest)
                .WithMany(g => g.Invitations)
                .HasForeignKey(i => i.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // ─────────────────────────────────────────
            // NOTIFICATION
            // ─────────────────────────────────────────

            // Notification → Recipient
            // Notification → Recipient
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict); // changed from Cascade to Restrict

            // Notification → Sender (nullable)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(n => n.SenderUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ─────────────────────────────────────────
            // DORMITORY SETTINGS
            // ─────────────────────────────────────────

            modelBuilder.Entity<DormitorySettings>()
                .Property(d => d.GuestRatePerNight)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DormitorySettings>()
                .Property(d => d.StudentMonthlyRate)
                .HasPrecision(18, 2);
        }
    }
}