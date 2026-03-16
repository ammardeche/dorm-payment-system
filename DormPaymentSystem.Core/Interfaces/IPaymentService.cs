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
        Task<Payment> ProcessPaymentAsync(int studentId, decimal amount, string paymentMethod);

        // CRUD with business logic
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(int id);

        // Helpers
        Task<bool> PaymentExistsAsync(int id);
        Task<bool> CanDeletePaymentAsync(int id);

    }
}