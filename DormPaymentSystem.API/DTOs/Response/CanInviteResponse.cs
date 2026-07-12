using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class CanInviteResponse
    {
        public bool CanInvite { get; set; }
        public string Message { get; set; } = null!;
    }
}