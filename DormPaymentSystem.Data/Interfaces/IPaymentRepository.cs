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
        Task<Payment> GetPaymentByStudentId(int studentId);
        Task<List<Payment>> GetPaymentByReceiptNumber(string receiptNumber);
        Task<List<Payment>> GetPaymentByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPendingPayments();
        Task<IEnumerable<Payment>> GetCompletedPayments();
        Task<Payment> CreatePayment(Payment payment);

        // Update & Delete Methods
        Task<Payment> UpdatePayment();
        Task<Payment> DeletePayment();
        // check if exists 
        Task<bool> PaymentExists(int id);

        Task SaveChanges();


    }
}