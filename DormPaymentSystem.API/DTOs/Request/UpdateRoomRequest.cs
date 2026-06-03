using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class UpdateRoomRequest
    {
        public int? RoomNumber { get; set; }
        public int? Capacity { get; set; }
        public RoomStatus? Status { get; set; }
    }
}