using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class AppCreateInvitationRequest
    {
        [Required] public string GuestFullName { get; set; } = null!;
        [Required] public string GuestNationalId { get; set; } = null!;
    }
}



