using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Interfaces;

namespace DormPaymentSystem.Core.Entities
{
    public class Student : IBuildingScoped
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DepartureNote { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int BuildingId {get;set; }

        // login account — manager creates this when registering student
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        // current room — nullable because student can be inactive
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        // navigation
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public Building Building { get; set; } = null!;
    }
}