using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IPaymentRepository
    {
        // READ — one method with filters
        Task<IEnumerable<Payment>> GetAllPayments(
            int? studentId = null,
            int? guestId = null,
            PaymentStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? receivedById = null,
            bool? dueToday = null);

        Task<Payment?> GetPaymentById(int id);
        Task<Payment?> GetPaymentByReceiptNumber(string receiptNumber);

        // WRITE
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(Payment payment); // soft delete

        // CHECK
        Task<bool> PaymentExists(int id);
        Task<bool> PaymentExistsForGuest(int guestId);

        Task<bool> PaymentExistsForMonth(int studentId, int month, int year);

        Task SaveChanges();




    }
}