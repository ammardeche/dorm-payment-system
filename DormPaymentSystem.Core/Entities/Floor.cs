using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class Floor
    {
        public int Id { get; set; }

        public int FloorNumber { get; set; }

        public int TotalRooms { get; set; } = 10;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}