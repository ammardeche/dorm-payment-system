using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class Guest
    {

        public int Id { get; set; }

        // identity only — no dates, no room, no payment
        // all stay details live on Reservation
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;

        // navigation
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}