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
        private readonly IStudentRepository _studentRepository;
        private readonly IGuestRepository _guestRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IStudentRepository studentRepository,
            IGuestRepository guestRepository
             )
        {
            _paymentRepository = paymentRepository;
            _studentRepository = studentRepository;
            _guestRepository = guestRepository;
        }
        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync(int? studentId = null,
        int? guestId = null, PaymentStatus? status = null, DateTime?
        startDate = null, DateTime? endDate = null, string? receivedById = null, bool? dueToday = null)
        {
            var payment = await _paymentRepository.GetAllPayments(
                 studentId: studentId,
                 guestId: guestId,
                 status: status,
                 startDate: startDate,
                 endDate: endDate,
                 receivedById: receivedById,
                 dueToday: dueToday
             );

            return payment;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetPaymentById(id);

            if (payment == null)
            {
                throw new AppNotFoundException("Payment doesn't exist");
            }

            return payment;
        }

        public async Task<Payment?> GetPaymentByReceiptNumberAsync(string receiptNumber)
        {
            var payment = await _paymentRepository.GetPaymentByReceiptNumber(receiptNumber);

            if (payment == null)
            {
                throw new AppNotFoundException($"Payment {receiptNumber} was not found.");
            }

            return payment;
        }

        public async Task<Payment> ProcessPaymentAsync(int? guestId, int? studentId, decimal amount, int? month, int? year, PaymentMethod method, string receivedByUserId)
        {
            // we need to use to flow for the student and for the guest , 
            // need to check at least one of them is not null
            if (studentId == null && guestId == null)
                throw new AppValidationException("Either studentId or GuestId must be provided.");

            if (studentId != null && guestId != null)
                throw new AppValidationException("Payment cannot be assigned to both a student and a guest.");

            // Shared validation
            if (amount <= 0)
                throw new AppValidationException("Amount must be greater than 0.");

            string receiptNumber;
            // '''' student flow ''''' 

            if (studentId != null)
            {

                var studentExists = await _studentRepository.StudentExists(studentId.Value);
                if (!studentExists)
                    throw new AppNotFoundException("Student doesn't exist.");

                if (month == null || year == null)
                    throw new AppValidationException("Month and year are required for student payments.");

                if (month < 1 || month > 12 || year < 2010)
                    throw new AppValidationException("Month must be 1–12 and year must be after 2010.");

                var alreadyPaid = await HasPaidForMonthAsync(studentId.Value, month.Value, year.Value);
                if (alreadyPaid)
                    throw new AppConflictException($"Payment for {month}/{year} already exists.");

                receiptNumber = await GenerateStudentReceiptNumber(studentId.Value);
            }

            else
            {
                var guest = await _guestRepository.GetGuestById(guestId.Value);
                if (guest == null)
                    throw new AppNotFoundException("Guest doesn't exist.");

                if (guest.CheckOutDate == null)
                    throw new AppValidationException("Guest must be checked out before payment.");

                var alreadyPaid = await _paymentRepository.PaymentExistsForGuest(guestId.Value);
                if (alreadyPaid)
                    throw new AppConflictException("Payment for this guest already exists.");


                receiptNumber = GenerateGuestReceiptNumber(guest.NationalId);
            }

            // ── CREATE PAYMENT ────────────────────────────────────────
            var payment = new Payment
            {
                StudentId = studentId,        // null if guest
                GuestId = guestId,            // null if student
                Amount = amount,
                PaymentMonth = month.Value,         // null if guest
                PaymentYear = year.Value,           // null if guest
                Method = method,
                ReceivedById = receivedByUserId,
                Status = PaymentStatus.Paid,
                CreatedAt = DateTime.UtcNow,
                ReceiptNumber = receiptNumber
            };

            return await _paymentRepository.CreatePayment(payment);

        }


        public async Task<Payment> UpdatePaymentAsync(int id, decimal amount, int month, int year, PaymentMethod method, PaymentStatus status, string? note)
        {
            // 1. Get existing payment
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
                throw new AppNotFoundException("Payment not found");

            // 2. Validate amount
            if (amount <= 0)
                throw new AppValidationException("Amount must be greater than 0");

            // 3. Validate month/year
            if (month < 1 || month > 12 || year < 2010)
                throw new AppValidationException("Invalid month or year");

            // 4. Prevent duplicate month/year for same student
            if (payment.PaymentMonth != month || payment.PaymentYear != year)
            {
                var exists = await HasPaidForMonthAsync(payment.StudentId.Value, month, year);
                if (exists)
                    throw new AppConflictException($"Payment for {month}/{year} already exists");
            }

            // 5. Update fields
            payment.Amount = amount;
            payment.PaymentMonth = month;
            payment.PaymentYear = year;
            payment.Method = method;
            payment.Status = status;
            payment.Note = note;
            payment.UpdatedAt = DateTime.UtcNow;

            // 6. Save
            return await _paymentRepository.UpdatePayment(payment);
        }

        public async Task<bool> DeletePaymentAsync(int id, string deletedById)
        {
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
                throw new AppNotFoundException("Payment not found");

            payment.DeletedById = deletedById;
            return await _paymentRepository.DeletePayment(payment);
        }

        public async Task<decimal> GetTotalCollectedAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                throw new AppValidationException("startDate should be less than end date ");
            }
            var payments = await _paymentRepository.GetAllPayments(
                startDate: startDate,
                endDate: endDate,
                status: PaymentStatus.Paid
            );

            return payments.Sum(x => x.Amount);
        }

        public async Task<bool> PaymentExistsAsync(int id)
        {
            return await _paymentRepository.PaymentExists(id);
        }

        public async Task<bool> HasPaidForMonthAsync(int studentId, int month, int year)
        {
            var paymentExist = await _paymentRepository.PaymentExistsForMonth(studentId, month, year);
            return paymentExist;
        }



        private async Task<string> GenerateStudentReceiptNumber(int studentId)
        {
            var student = await _studentRepository.GetStudentById(studentId);

            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string studentPart = $"STD{student?.StudentNumber}";
            string randomPart = new Random().Next(1000, 9999).ToString();
            return $"RCPT-{datePart}-{studentPart}-{randomPart}";
        }

        private string GenerateGuestReceiptNumber(string nationalId)
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomPart = new Random().Next(1000, 9999).ToString();
            return $"RCPT-{datePart}-GST{nationalId}-{randomPart}";
        }

        public Task<bool> PaymentExistsForGuest(int guestId)
        {
            return _paymentRepository.PaymentExistsForGuest(guestId);
        }
    }



}

