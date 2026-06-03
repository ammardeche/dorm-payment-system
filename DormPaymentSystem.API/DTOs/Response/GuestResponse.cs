using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class GuestResponse
    {

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;

        // stay history — all reservations for this guest
        public IEnumerable<GuestReservationResponse> Reservations { get; set; }

        public GuestResponse(Guest guest)
        {
            Id = guest.Id;
            FullName = guest.FullName;
            NationalId = guest.NationalId;

            Reservations = guest.Reservations.Select(r => new GuestReservationResponse
            {
                Id = r.Id,
                Type = r.Type,
                Status = r.Status,
                RoomNumber = r.Room?.RoomNumber,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                TotalAmount = r.TotalAmount
            });
        }
    }

    public class GuestReservationResponse
    {
        public int Id { get; set; }
        public ReservationType Type { get; set; }
        public ReservationStatus Status { get; set; }
        public int? RoomNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal TotalAmount { get; set; }

    }
}