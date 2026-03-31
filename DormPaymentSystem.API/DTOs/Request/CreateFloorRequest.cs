using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateFloorRequest
    {

        public int FloorNumber { get; set; }

        public int TotalRooms { get; set; }

    }
}