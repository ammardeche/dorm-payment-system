using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string? DepartureNote { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public DateTime? DepartureDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int? RoomId { get; set; }

        public Room? Room { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}