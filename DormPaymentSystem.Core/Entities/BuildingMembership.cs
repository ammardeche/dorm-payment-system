using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class BuildingMembership
    {

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public int BuildingId { get; set; }
        public Building Building { get; set; } = null!;
    }
}