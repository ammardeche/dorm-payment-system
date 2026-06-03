using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateInvitationRequest
    {
        [Required] public int InvitedByStudentId { get; set; }
        [Required] public int GuestId { get; set; }

    }
}