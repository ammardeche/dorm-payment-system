using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class StudentResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string StudentNumber { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public IEnumerable<StudentPaymentResponse> Payments { get; set; }
        public StudentRoomResponse? Room { get; set; }


        public StudentResponse(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            StudentNumber = student.StudentNumber;
            PhoneNumber = student.PhoneNumber;
            EnrollmentDate = student.EnrollmentDate;
            Payments = student.Payments.Select(p => new StudentPaymentResponse
            {
                Amount = p.Amount,
                PaymentDate = p.PaymentDate
            });
            Room = student.Room != null ? new StudentRoomResponse
            {
                Id = student.Room.Id,
                RoomNumber = student.Room.RoomNumber ?? 0
            } : null;
        }

    }

    public class StudentPaymentResponse
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class StudentRoomResponse
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }

    }
}