using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.Queries
{
    public class InvitationQuery
    {

        public int? RoomId { get; set; }
        public int? StudentId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

    }
}