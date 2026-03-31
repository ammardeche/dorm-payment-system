using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IFloorService
    {
        Task<IEnumerable<Floor>> GetAllFloorsAsync();
        Task<Floor> CreateFloorAsync(int floorNumber, int totalRooms);       // throws if duplicate
        Task<Floor> UpdateFloorAsync(int floorNumber, int totalRooms);       // throws if not found
        Task<bool> DeleteFloorAsync(int floorNumber);            // throws if has rooms
        Task<bool> IsFloorExistsAsync(int floorNumber);

    }
}