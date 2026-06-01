using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateInvitationReservationRequest
    {
        [Required] public int InvitationId { get; set; }
        [Required] public int RoomId { get; set; }
        [Required] public DateTime CheckIn { get; set; }
        [Required] public DateTime CheckOut { get; set; }
    }
}