using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPayments();
        Task<Payment?> GetPaymentById(int id);
        Task<IEnumerable<Payment>> GetPaymentByStudentId(int studentId);
        Task<IEnumerable<Payment>> GetPaymentByGuestId(int guestId);      // added
        Task<Payment?> GetPaymentByReceiptNumber(string receiptNumber);
        Task<IEnumerable<Payment>> GetPaymentByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPendingPayments();
        Task<IEnumerable<Payment>> GetOverduePayments();
        Task<IEnumerable<Payment>> GetDueTodayPayments();
        Task<IEnumerable<Payment>> GetPaymentsByReceptionist(string userId);

        // WRITE
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(int id);                                  // fixed

        // CHECK
        Task<bool> PaymentExists(int id);
        Task<bool> PaymentExistsForMonth(int studentId, int month, int year);

        Task SaveChanges();


    }
}