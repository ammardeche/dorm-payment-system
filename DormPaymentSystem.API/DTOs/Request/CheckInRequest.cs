using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CheckInRequest
    {
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public int RoomId { get; set; }
        public decimal RatePerNight { get; set; }
        public int? NightsStayed { get; set; }
    }
}