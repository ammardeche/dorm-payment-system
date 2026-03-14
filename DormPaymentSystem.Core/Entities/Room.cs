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

        public int Capacity { get; set; }

        public int CurrentOccupancy { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public int FloorId { get; set; }
        public Floor Floor { get; set; } = null!;
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }
}