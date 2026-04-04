using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRooms(
            int? floorId,
            RoomStatus? status
        );
        Task<Room?> GetRoomById(int id);
        Task<Room> CreateRoom(Room room);
        Task<Room> UpdateRoom(Room room);
        Task<bool> DeleteRoom(Room room);

        Task<bool> RoomNumberExist(int roomNumber);
        Task<bool> RoomExists(int id);
        Task SaveChanges();
    }
}