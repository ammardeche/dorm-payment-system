using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class GuestResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public int RoomId { get; set; }
        public decimal RatePerNight { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int NightsStayed { get; set; }
        public decimal TotalAmount { get; set; }

        public GuestResponse(Guest guest)
        {
            Id = guest.Id;
            FullName = guest.FullName;
            NationalId = guest.NationalId;
            RoomId = guest.RoomId.Value;
            RatePerNight = guest.RatePerNight;
            CheckInDate = guest.CheckInDate;
            CheckOutDate = guest.CheckOutDate;
            NightsStayed = guest.NightsStayed;
            TotalAmount = guest.TotalAmount;
        }
    }
}