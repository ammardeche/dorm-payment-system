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
        public string ReceiptNumber { get; set; }
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

        public string? ReceivedById { get; set; }
        public UserResponse? ReceivedBy { get; set; }

        public int? StudentId { get; set; }
        public PaymentStudentResponse? Student { get; set; }

        public int? GuestId { get; set; }
        public PaymentGuestResponse? Guest { get; set; }

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

            ReceivedById = payment.ReceivedById;
            ReceivedBy = payment.ReceivedBy != null
                ? new UserResponse(payment.ReceivedBy)
                : null;

            Student = payment.Student != null
                ? new PaymentStudentResponse(payment.Student)
                : null;

            Guest = payment.Guest != null
                ? new PaymentGuestResponse(payment.Guest)
                : null;
        }
    }

    public class PaymentStudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int? RoomNumber { get; set; }

        public PaymentStudentResponse(Student student)
        {
            Id = student.Id;
            FullName = $"{student.FirstName} {student.LastName}";
            RoomNumber = student.Room?.RoomNumber;
        }
    }

    public class PaymentGuestResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public PaymentGuestResponse(Guest guest)
        {
            Id = guest.Id;
            FullName = guest.FullName;
        }
    }

    public class UserResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public UserResponse(User user)
        {
            Id = user.Id;
            FullName = $"{user.FirstName} {user.LastName}";
        }
    }
}