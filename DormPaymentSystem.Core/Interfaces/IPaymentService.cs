using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IPaymentService
    {
        // READ
        Task<IEnumerable<Payment>> GetAllPaymentsAsync(
            int? studentId = null,
            int? guestId = null,
            PaymentStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? receivedById = null,
            bool? dueToday = null);

        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<Payment?> GetPaymentByReceiptNumberAsync(string receiptNumber);

        // WRITE
        Task<Payment> ProcessPaymentAsync(
            int studentId,
            decimal amount,
            int month,
            int year,
            PaymentMethod method,
            string receivedByUserId);

        Task<Payment> UpdatePaymentAsync(
            int id,
            decimal amount,
            int month,
            int year,
            PaymentMethod method,
            PaymentStatus status,
            string? note);

        Task<bool> DeletePaymentAsync(int id, string deletedById);

        // REPORTING
        Task<decimal> GetTotalCollectedAsync(DateTime startDate, DateTime endDate);

        // VALIDATION
        Task<bool> PaymentExistsAsync(int id);
        Task<bool> HasPaidForMonthAsync(int studentId, int month, int year);

    }
}