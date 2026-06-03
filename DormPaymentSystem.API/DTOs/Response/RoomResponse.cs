using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
        public int FloorNumber { get; set; }        // simple int now
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
        public RoomStatus Status { get; set; }

        // active students in this room
        public IEnumerable<RoomStudentResponse> Students { get; set; }

        // active reservations (guest or invitation stays)
        public IEnumerable<RoomReservationResponse> ActiveReservations { get; set; }

        public RoomResponse(Room room)
        {
            Id = room.Id;
            RoomNumber = room.RoomNumber;
            FloorNumber = room.FloorNumber;
            Capacity = room.Capacity;
            CurrentOccupancy = room.Students.Count(s => s.IsActive);
            Status = room.Status;

            Students = room.Students
                .Where(s => s.IsActive)
                .Select(s => new RoomStudentResponse
                {
                    Id = s.Id,
                    FullName = $"{s.FirstName} {s.LastName}",
                    StudentNumber = s.StudentNumber
                });

            ActiveReservations = room.Reservations
                .Where(r => r.Status == ReservationStatus.Active)
                .Select(r => new RoomReservationResponse
                {
                    Id = r.Id,
                    Type = r.Type,
                    CheckIn = r.CheckIn,
                    CheckOut = r.CheckOut,
                    GuestName = r.Guest?.FullName
                });
        }
    }

    public class RoomStudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
    }

    public class RoomReservationResponse
    {
        public int Id { get; set; }
        public ReservationType Type { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? GuestName { get; set; }
    }

}




