using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IRoomService
    {
        // READ
        Task<IEnumerable<Room>> GetAllRooms(RoomStatus? status, int? floorId);
        Task<Room?> GetRoomById(int id);
        // WRITE
        Task<Room> CreateRoom(int roomNumber, int capacity, int floorId);
        Task<Room> UpdateRoom(int id, int? roomNumber, int capacity, RoomStatus status);
        Task<bool> DeleteRoom(int id);
        // BUSINESS LOGIC
        Task<bool> IsRoomAvailable(int roomId);        // checks capacity
    }
}