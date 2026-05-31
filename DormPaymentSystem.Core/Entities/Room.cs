using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
        public int FloorNumber { get; set; }  // replaces Floor entity
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        // navigation
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}