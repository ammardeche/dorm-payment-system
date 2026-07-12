using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IDormitorySettingsRepository _settingsRepository;

        // no longer needs IStudentRepository or IGuestRepository
        // payment only knows its ReservationId
        public PaymentService(
            IDormitorySettingsRepository settingsRepository,
            IPaymentRepository paymentRepository,
            IReservationRepository reservationRepository)
        {
            _paymentRepository = paymentRepository;
            _reservationRepository = reservationRepository;
            _settingsRepository = settingsRepository;
        }

        public async Task<(IEnumerable<Payment> Items, int TotalCount)> GetAllPaymentsAsync(
            int? reservationId = null,
            PaymentStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? receivedById = null,
            bool? dueToday = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _paymentRepository.GetAllPayments(
                reservationId, status, startDate,
                endDate, receivedById, dueToday,
                pageIndex, pageSize);
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
                throw new AppNotFoundException("Payment not found.");
            return payment;
        }

        public async Task<Payment?> GetPaymentByReceiptNumberAsync(string receiptNumber)
        {
            var payment = await _paymentRepository.GetPaymentByReceiptNumber(receiptNumber);
            if (payment == null)
                throw new AppNotFoundException($"Payment '{receiptNumber}' not found.");
            return payment;
        }

        // receptionist marks an existing pending payment as paid
        // payments are created by ReservationService — not here
        public async Task<Payment> MarkAsPaidAsync(
            int paymentId,
            PaymentMethod method,
            string receivedByUserId)
        {
            var payment = await _paymentRepository.GetPaymentById(paymentId);
            if (payment == null)
                throw new AppNotFoundException("Payment not found.");

            if (payment.Status == PaymentStatus.Paid)
                throw new AppConflictException("Payment is already paid.");

            // calculate days late
            var daysLate = (int)Math.Max(
                0, (DateTime.UtcNow - payment.DueDate).TotalDays);

            payment.Status = PaymentStatus.Paid;
            payment.Method = method;
            payment.ReceivedById = receivedByUserId;
            payment.PaymentDate = DateTime.UtcNow;
            payment.DaysLate = daysLate;
            payment.UpdatedAt = DateTime.UtcNow;

            return await _paymentRepository.UpdatePayment(payment);
        }

        public async Task<Payment> UpdatePaymentAsync(
            int id,
            decimal? amount,
            PaymentMethod? method,
            PaymentStatus? status,
            string? note)
        {
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
                throw new AppNotFoundException("Payment not found.");

            if (amount.HasValue)
            {
                if (amount.Value <= 0)
                    throw new AppValidationException("Amount must be greater than 0.");
                payment.Amount = amount.Value;
            }

            if (method.HasValue) payment.Method = method.Value;
            if (status.HasValue) payment.Status = status.Value;
            if (note != null) payment.Note = note;

            payment.UpdatedAt = DateTime.UtcNow;

            return await _paymentRepository.UpdatePayment(payment);
        }

        public async Task<bool> DeletePaymentAsync(int id, string deletedById)
        {
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
                throw new AppNotFoundException("Payment not found.");

            if (payment.Status == PaymentStatus.Paid)
                throw new AppValidationException("Cannot delete a paid payment.");

            payment.DeletedById = deletedById;
            return await _paymentRepository.DeletePayment(payment);
        }

        public async Task<decimal> GetTotalCollectedAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new AppValidationException("Start date must be before end date.");

            var (payments, _) = await _paymentRepository.GetAllPayments(
                startDate: startDate,
                endDate: endDate,
                status: PaymentStatus.Paid,
                pageSize: 100);

            return payments.Sum(p => p.Amount);
        }

        public async Task<bool> PaymentExistsAsync(int id)
        {
            return await _paymentRepository.PaymentExists(id);
        }

        public async Task<bool> HasPaidForMonthAsync(int reservationId, int month, int year)
        {
            return await _paymentRepository.PaymentExistsForMonth(
                reservationId, month, year);
        }

        public async Task RebuildPaymentsAsync(int reservationId)
        {
            await EnsureMonthlyPaymentsAsync(reservationId);
        }
        public async Task EnsureMonthlyPaymentsAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationById(reservationId);
            if (reservation == null) return;

            if (reservation.Type != ReservationType.StudentStay)
                return;

            var settings = await _settingsRepository.GetCurrentSettings();
            if (settings == null) return;

            var start = new DateTime(reservation.CheckIn.Year, reservation.CheckIn.Month, 1);
            var now = DateTime.UtcNow;
            var end = new DateTime(now.Year, now.Month, 1);

            for (var date = start; date <= end; date = date.AddMonths(1))
            {
                var exists = await _paymentRepository
                    .PaymentExistsForMonth(reservation.Id, date.Month, date.Year);

                if (exists) continue;

                await _paymentRepository.CreatePayment(new Payment
                {
                    ReservationId = reservation.Id,
                    Amount = reservation.RateSnapshot,
                    PaymentMonth = date.Month,
                    PaymentYear = date.Year,
                    DueDate = date,
                    Status = PaymentStatus.Pending,
                    ReceiptNumber = $"AUTO-{date:yyyyMM}-{reservation.StudentId}",
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

    }


}

