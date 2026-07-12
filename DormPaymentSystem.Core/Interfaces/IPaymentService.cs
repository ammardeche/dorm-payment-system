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
        Task<(IEnumerable<Payment> Items, int TotalCount)> GetAllPaymentsAsync(
            int? reservationId = null,
            PaymentStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? receivedById = null,
            bool? dueToday = null,
            int pageIndex = 1,
            int pageSize = 10);

        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<Payment?> GetPaymentByReceiptNumberAsync(string receiptNumber);

        // mark an existing pending payment as paid
        Task<Payment> MarkAsPaidAsync(
            int paymentId,
            PaymentMethod method,
            string receivedByUserId);

        Task<Payment> UpdatePaymentAsync(
            int id,
            decimal? amount,
            PaymentMethod? method,
            PaymentStatus? status,
            string? note);

        Task<bool> DeletePaymentAsync(int id, string deletedById);

        Task<decimal> GetTotalCollectedAsync(DateTime startDate, DateTime endDate);

        Task<bool> PaymentExistsAsync(int id);
        Task<bool> HasPaidForMonthAsync(int reservationId, int month, int year);

        Task RebuildPaymentsAsync(int reservationId);
        Task EnsureMonthlyPaymentsAsync(int reservationId);
    }
}