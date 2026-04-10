using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;

        public DateTime CheckInDate { get; set; } = DateTime.UtcNow;
        public DateTime? CheckOutDate { get; set; }

        public int NightsStayed { get; set; }

        // snapshot of price at time of stay
        public decimal RatePerNight { get; set; }
        public decimal TotalAmount { get; set; }


        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}