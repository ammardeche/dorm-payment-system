using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateRoomRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Room number must be greater than 0.")]
        public int RoomNumber { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10.")]
        public int Capacity { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Floor id must be greater than 0.")]
        public int FloorId { get; set; }
    }
}