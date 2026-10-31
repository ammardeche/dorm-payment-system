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
        Task<(IEnumerable<Room> Items, int TotalCount)> GetAllRoomsAsync(
            RoomStatus? status = null,
            int? floorNumber = null,
            int pageIndex = 1,
            int pageSize = 10);

        Task<Room?> GetRoomByIdAsync(int id);
        Task<Room> CreateRoomAsync(int roomNumber, int capacity, int floorNumber);
        Task<Room> UpdateRoomAsync(int id, int? roomNumber, int? capacity, RoomStatus? status);
        Task<bool> DeleteRoomAsync(int id);
        Task<RoomStatsResponse> GetRoomStatsAsync();
        Task<bool> IsRoomAvailableAsync(int roomId);
    }
}