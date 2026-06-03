using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateRoomRequest
    {
        [Required] public int RoomNumber { get; set; }
        [Required] public int Capacity { get; set; }
        [Required] public int FloorNumber { get; set; }  // replaces FloorId
    }
}