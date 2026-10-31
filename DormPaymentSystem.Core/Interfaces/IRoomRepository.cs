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
        Task<(IEnumerable<Room> Items, int TotalCount)> GetAllRooms(
            RoomStatus? status = null,
            int? floorNumber = null,        // int replaces floorId
            int pageIndex = 1,
            int pageSize = 10);

        Task<Room?> GetRoomById(int id);
        Task<Room> CreateRoom(Room room);
        Task<Room> UpdateRoom(Room room);
        Task<bool> DeleteRoom(Room room);
        Task<bool> RoomExists(int id);
        Task<bool> RoomNumberExist(int roomNumber);
        Task<RoomStatsResponse> GetRoomStats();
        Task SaveChanges();
    }
}