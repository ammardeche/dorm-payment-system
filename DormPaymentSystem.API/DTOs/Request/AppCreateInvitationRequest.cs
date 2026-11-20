using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class AppCreateInvitationRequest
    {
        [Required] public string GuestFullName { get; set; } = null!;
        [Required] public string GuestDocumentNumber { get; set; } = null!;
        [Required] public DocumentType DocumentType { get; set; }
    }
}



