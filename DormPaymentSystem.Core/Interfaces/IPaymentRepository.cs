using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IPaymentRepository
    {
        // what i need to do with payment repository 
        // Get all payments , get payment by student , create payment , update payment , delete payment , get payment by date range 

        // read Methods
        Task<IEnumerable<Payment>> GetAllPayments();
        // filter
        Task<Payment> GetPaymentById(int id);
        Task<IEnumerable<Payment>> GetPaymentByStudentId(int studentId);
        Task<Payment> GetPaymentByReceiptNumber(string receiptNumber);
        Task<IEnumerable<Payment>> GetPaymentByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPendingPayments();
        Task<IEnumerable<Payment>> GetOverduePayments();
        Task<IEnumerable<Payment>> GetDueTodayPayments();
        Task<IEnumerable<Payment>> GetPaymentsByReceptionist(string userId);

        //Create & Update & Delete Methods
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
        Task<bool> DeletePayment();
        // check if exists 
        Task<bool> PaymentExists(int id);
        Task<bool> PaymentExistsForMonth(int studentId, int month, int year); // ✅ Add - prevent duplicate payments


        Task SaveChanges();


    }
}