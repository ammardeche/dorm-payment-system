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
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
        public RoomStatus Status { get; set; }
        public RoomFloorResponse? Floor { get; set; }
        public IEnumerable<RoomStudentResponse> Students { get; set; }
        public IEnumerable<RoomGuestResponse> Guests { get; set; }
        public IEnumerable<RoomInvitationResponse> Invitations { get; set; }

        public RoomResponse(Room room)
        {
            Id = room.Id;
            RoomNumber = room.RoomNumber;
            Capacity = room.Capacity;

            // ✅ calculate from actual active students not stored value
            CurrentOccupancy = room.Students.Count(s => s.IsActive);

            Status = room.Status;

            Floor = room.Floor != null ? new RoomFloorResponse
            {
                FloorNumber = room.Floor.FloorNumber
            } : null;

            Students = room.Students
                .Where(s => s.IsActive)   // show only active students
                .Select(s => new RoomStudentResponse
                {
                    FirstName = s.FirstName,
                    StudentNumber = s.StudentNumber
                });

            Guests = room.Guests.Select(g => new RoomGuestResponse
            {
                FullName = g.FullName,
                NationalId = g.NationalId
            });

            Invitations = room.Invitations.Select(i => new RoomInvitationResponse
            {
                GuestName = i.GuestName,
                GuestIdentityId = i.GuestIdentityId
            });
        }
    }

    public class RoomFloorResponse
    {
        public int FloorNumber { get; set; }
    }

    public class RoomStudentResponse
    {
        public string? FirstName { get; set; }
        public string? StudentNumber { get; set; }
    }

    public class RoomGuestResponse
    {
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
    }

    public class RoomInvitationResponse
    {
        public string? GuestName { get; set; }
        public string? GuestIdentityId { get; set; }
    }
}