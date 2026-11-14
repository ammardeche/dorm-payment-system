using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.Queries
{
    public class RoomQuery
    {
        public RoomStatus? Status { get; set; }
        public int? FloorNumber { get; set; }   // replaces FloorId
        public string? RoomNumber { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}