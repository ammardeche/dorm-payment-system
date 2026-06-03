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

        public async Task<(IEnumerable<Payment> Items, int TotalCount)> GetAllPayments(
            int? reservationId = null,
            PaymentStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? receivedById = null,
            bool? dueToday = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Payments
                .AsNoTracking()
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Student)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Guest)
                .Include(p => p.ReceivedBy)
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (reservationId.HasValue)
                query = query.Where(p => p.ReservationId == reservationId.Value);

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

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Payment?> GetPaymentById(int id)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Student)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Guest)
                .Include(p => p.ReceivedBy)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Payment?> GetPaymentByReceiptNumber(string receiptNumber)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Student)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Guest)
                .Include(p => p.ReceivedBy)
                .FirstOrDefaultAsync(p => p.ReceiptNumber == receiptNumber && !p.IsDeleted);
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await SaveChanges();
            return payment;
        }

        public async Task<Payment> UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            await SaveChanges();
            return payment;
        }

        public async Task<bool> DeletePayment(Payment payment)
        {
            payment.IsDeleted = true;
            payment.DeletedAt = DateTime.UtcNow;
            _context.Payments.Update(payment);
            await SaveChanges();
            return true;
        }

        public async Task<bool> PaymentExists(int id)
        {
            return await _context.Payments
                .AnyAsync(p => p.Id == id && !p.IsDeleted);
        }

        // now checks by reservationId instead of studentId
        public async Task<bool> PaymentExistsForMonth(int reservationId, int month, int year)
        {
            return await _context.Payments
                .AnyAsync(p =>
                    p.ReservationId == reservationId &&
                    p.PaymentMonth == month &&
                    p.PaymentYear == year &&
                    !p.IsDeleted);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
