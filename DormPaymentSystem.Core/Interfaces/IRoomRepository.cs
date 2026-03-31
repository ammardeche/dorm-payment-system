using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IRoomRepository
    {
        // READ
        Task<IEnumerable<Room>> GetAllRooms();
        Task<Room> GetRoomById(int id);
        Task<Room> GetRoomByFloorAndNumber(int floorId, int roomNumber);

        // FILTER
        Task<IEnumerable<Room>> GetAvailableRooms();
        Task<IEnumerable<Room>> GetRoomsByFloor(int floorId);
        Task<IEnumerable<Room>> GetFullRooms();

        // CREATE
        Task<Room> CreateRoom(Room room);

        // UPDATE
        Task<Room> UpdateRoom(Room room);

        // DELETE
        Task<bool> DeleteRoom(int id);

        // CHECK
        Task<bool> RoomExists(int id);

        // SAVE
        Task SaveChanges();
    }
}