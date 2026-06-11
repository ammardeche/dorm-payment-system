using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Repositories;

namespace DormPaymentSystem.Core.Jobs
{
    public class PaymentGenerationJob : IPaymentGenerationJob
    {

        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IDormitorySettingsRepository _settingsRepository;

        public PaymentGenerationJob(
            IReservationRepository reservationRepository,
            IPaymentRepository paymentRepository,
            IDormitorySettingsRepository settingsRepository)
        {
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
            _settingsRepository = settingsRepository;
        }

        public async Task ExecuteAsync()
        {
            // get current month and year automatically
            var now = DateTime.UtcNow;
            var month = now.Month;
            var year = now.Year;

            // get pricing
            var settings = await _settingsRepository.GetCurrentSettings();
            if (settings == null) return; // no pricing configured — skip

            // get all active student reservations
            var (reservations, _) = await _reservationRepository.GetAllReservations(
                type: ReservationType.StudentStay,
                status: ReservationStatus.Active,
                pageSize: 1000);

            foreach (var reservation in reservations)
            {
                // skip if payment already exists for this month
                // protects against running twice
                var alreadyExists = await _paymentRepository
                    .PaymentExistsForMonth(reservation.Id, month, year);
                if (alreadyExists) continue;

                var payment = new Payment
                {
                    ReservationId = reservation.Id,
                    Amount = reservation.RateSnapshot, // locked price at check-in
                    PaymentMonth = month,
                    PaymentYear = year,
                    DueDate = new DateTime(year, month, 1),
                    Status = PaymentStatus.Pending,
                    ReceiptNumber = $"AUTO-{year}{month:D2}-{reservation.StudentId}",
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentRepository.CreatePayment(payment);
            }
        }
    }
}
