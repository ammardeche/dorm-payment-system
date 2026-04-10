using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DormPaymentDbContext _context;

        public PaymentRepository(DormPaymentDbContext context)
        {
            _context = context;
        }
        public async Task<Payment> CreatePayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await SaveChanges();
            return payment;
        }

        public async Task<bool> DeletePayment(Payment payment)
        {
            // soft delete — never remove from DB
            payment.IsDeleted = true;
            payment.DeletedAt = DateTime.UtcNow;
            _context.Payments.Update(payment);
            await SaveChanges();
            return true;

        }

        public async Task<IEnumerable<Payment>> GetAllPayments(int? studentId = null, int? guestId = null, PaymentStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, string? receivedById = null, bool? dueToday = null)
        {
            var query = _context.Payments
           .AsNoTracking()
           .Include(p => p.Student)
           .Include(p => p.Guest)
           .Include(p => p.ReceivedBy)
           .Where(p => p.IsDeleted == false)
           .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(p => p.StudentId == studentId.Value);

            if (guestId.HasValue)
                query = query.Where(p => p.GuestId == guestId.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (startDate.HasValue)
                query = query.Where(p => p.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.CreatedAt <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(receivedById))
                query = query.Where(p => p.ReceivedById == receivedById);

            if (dueToday.HasValue && dueToday.Value)
                query = query.Where(p => p.DueDate.Date == DateTime.UtcNow.Date);


            return await query.ToListAsync();
        }

        public async Task<Payment?> GetPaymentById(int id)
        {
            return await _context.Payments
             .Include(p => p.Student)
             .Include(p => p.Guest)
             .Include(p => p.ReceivedBy)
             .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<Payment?> GetPaymentByReceiptNumber(string receiptNumber)
        {
            return await _context.Payments
             .Include(p => p.Student)
             .Include(p => p.Guest)
             .Include(p => p.ReceivedBy)
             .FirstOrDefaultAsync(p => p.ReceiptNumber == receiptNumber && p.IsDeleted == false);

        }

        public async Task<bool> PaymentExists(int id)
        {
            return await _context.Payments
           .AnyAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<bool> PaymentExistsForMonth(int studentId, int month, int year)
        {
            return await _context.Payments
           .AnyAsync(p =>
               p.StudentId == studentId &&
               p.PaymentMonth == month &&
               p.PaymentYear == year &&
               p.IsDeleted == false);
        }


        public async Task<Payment> UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await SaveChanges();
            return payment;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PaymentExistsForGuest(int guestId)
        {
            return await _context.Payments
                .AnyAsync(p => p.GuestId == guestId && p.Status == PaymentStatus.Paid);
        }
    }
}