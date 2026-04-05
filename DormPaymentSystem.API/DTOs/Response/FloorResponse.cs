using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs
{
    public class FloorResponse
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public int TotalRooms { get; set; }
        public IEnumerable<FloorRoomResponse> Rooms { get; set; }

        public FloorResponse(Floor floor)
        {
            Id = floor.Id;
            FloorNumber = floor.FloorNumber;
            TotalRooms = floor.TotalRooms;
            Rooms = floor.Rooms.Select(r => new FloorRoomResponse
            {
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                CurrentOccupancy = r.CurrentOccupancy
            });
        }
    }

    // This is the nested room inside FloorResponse
    public class FloorRoomResponse
    {
        public int? RoomNumber { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
    }
}