using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ReceiptNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod? Method { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public int DaysLate { get; set; }

        // who recorded this payment
        public PaymentReceivedByResponse? ReceivedBy { get; set; }

        // reservation context — who paid and for what
        public PaymentReservationResponse? Reservation { get; set; }

        public PaymentResponse(Payment payment)
        {
            Id = payment.Id;
            Amount = payment.Amount;
            ReceiptNumber = payment.ReceiptNumber;
            PaymentDate = payment.PaymentDate;
            DueDate = payment.DueDate;
            Status = payment.Status;
            Method = payment.Method;
            Note = payment.Note;
            CreatedAt = payment.CreatedAt;
            UpdatedAt = payment.UpdatedAt;
            PaymentMonth = payment.PaymentMonth;
            PaymentYear = payment.PaymentYear;
            DaysLate = payment.DaysLate;

            ReceivedBy = payment.ReceivedBy != null
                ? new PaymentReceivedByResponse
                {
                    Id = payment.ReceivedBy.Id,
                    FullName = payment.ReceivedBy.FullName
                }
                : null;

            // reservation tells us who paid and what type
            Reservation = payment.Reservation != null
                ? new PaymentReservationResponse
                {
                    Id = payment.Reservation.Id,
                    Type = payment.Reservation.Type,

                    // student name if StudentStay
                    StudentName = payment.Reservation.Student != null
                        ? $"{payment.Reservation.Student.FirstName} {payment.Reservation.Student.LastName}"
                        : null,
                    StudentNumber = payment.Reservation.Student?.StudentNumber,

                    // guest name if GuestStay or InvitationStay
                    GuestName = payment.Reservation.Guest?.FullName
                }
                : null;
        }
    }

    public class PaymentReceivedByResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }

    public class PaymentReservationResponse
    {
        public int Id { get; set; }
        public ReservationType Type { get; set; }
        public string? StudentName { get; set; }
        public string? StudentNumber { get; set; }
        public string? GuestName { get; set; }
    }
}
