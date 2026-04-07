using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Repositories;

namespace DormPaymentSystem.Core.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public Task<IEnumerable<Payment>> GetAllPaymentsAsync(int? studentId = null, int? guestId = null, PaymentStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, string? receivedById = null, bool? dueToday = null)
        {
            throw new NotImplementedException();
        }

        public Task<Payment?> GetPaymentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Payment?> GetPaymentByReceiptNumberAsync(string receiptNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> ProcessPaymentAsync(int studentId, decimal amount, int month, int year, PaymentMethod method, string receivedByUserId)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> UpdatePaymentAsync(int id, decimal amount, int month, int year, PaymentMethod method, PaymentStatus status, string? note)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePaymentAsync(int id, string deletedById)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetTotalCollectedAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PaymentExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPaidForMonthAsync(int studentId, int month, int year)
        {
            throw new NotImplementedException();
        }



    }
}