using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.Queries
{
    public class ReservationQuery
    {
        public int? StudentId { get; set; }
        public int? GuestId { get; set; }
        public int? RoomId { get; set; }
        public ReservationType? Type { get; set; }
        public ReservationStatus? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}