using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public bool IsActive { get; set; }

        public StudentRoomResponse? Room { get; set; }

        // payments reached via reservations
        public IEnumerable<StudentPaymentResponse> Payments { get; set; }

        public StudentResponse(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            StudentNumber = student.StudentNumber;
            Email = student.Email;
            PhoneNumber = student.PhoneNumber;
            EnrollmentDate = student.EnrollmentDate;
            DepartureDate = student.DepartureDate;
            IsActive = student.IsActive;

            Room = student.Room != null
                ? new StudentRoomResponse
                {
                    Id = student.Room.Id,
                    RoomNumber = student.Room.RoomNumber ?? 0,
                    FloorNumber = student.Room.FloorNumber
                }
                : null;

            // flatten payments from all student reservations
            Payments = student.Reservations
                .Where(r => r.Type == ReservationType.StudentStay)
                .SelectMany(r => r.Payments)
                .Select(p => new StudentPaymentResponse
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    PaidMonth = $"{p.PaymentMonth}/{p.PaymentYear}",
                    Status = p.Status,
                    DueDate = p.DueDate,
                    PaymentDate = p.PaymentDate
                });
        }
    }

    public class StudentRoomResponse
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
    }

    public class StudentPaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaidMonth { get; set; } = null!;
        public PaymentStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}