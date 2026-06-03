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
        Task<(IEnumerable<Payment> Items, int TotalCount)> GetAllPayments(
               int? reservationId = null,
               PaymentStatus? status = null,
               DateTime? startDate = null,
               DateTime? endDate = null,
               string? receivedById = null,
               bool? dueToday = null,
               int pageIndex = 1,
               int pageSize = 10);

        Task<Payment?> GetPaymentById(int id);
        Task<Payment?> GetPaymentByReceiptNumber(string receiptNumber);

        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(Payment payment);

        Task<bool> PaymentExists(int id);
        Task<bool> PaymentExistsForMonth(int reservationId, int month, int year);

        Task SaveChanges();
    }
}