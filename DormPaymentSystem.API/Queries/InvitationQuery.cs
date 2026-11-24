using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.Queries
{
    public class InvitationQuery
    {
        public int? StudentId { get; set; }
        public int? GuestId { get; set; }      // replaces RoomId
        public InvitationStatus? InvitationStatus { get; set; } = null;
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}