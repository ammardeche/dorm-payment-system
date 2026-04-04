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
        public int? FloorId { get; set; }
    }
}