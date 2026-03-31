using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IPaymentService
    {
        // Main business operation
        Task<Payment> ProcessPaymentAsync(int studentId, decimal amount, string paymentMethod, string receivedByUserId);

        // CRUD with business logic
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(int id);

        // REPORTING & QUERIES               // ✅ Add these - you'll need them
        Task<IEnumerable<Payment>> GetStudentPaymentHistoryAsync(int studentId);
        Task<IEnumerable<Payment>> GetOverduePaymentsAsync();
        Task<IEnumerable<Payment>> GetDueTodayAsync();
        Task<decimal> GetTotalCollectedAsync(DateTime startDate, DateTime endDate);

        // VALIDATION
        Task<bool> PaymentExistsAsync(int id);
        Task<bool> CanDeletePaymentAsync(int id);
        Task<bool> HasPaidForMonthAsync(int studentId, int month, int year); // ✅ Add - prevent double payment

    }
}