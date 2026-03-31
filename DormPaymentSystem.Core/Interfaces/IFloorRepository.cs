using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IFloorRepository
    {
        Task<IEnumerable<Floor>> GetAllFloors();
        Task<Floor> GetFloorByFloorNumber(int floorNumber);
        Task<Floor> CreateFloor(Floor floor);
        Task<Floor> UpdateFloor(Floor floor);
        Task<bool> DeleteFloor(Floor floor);

        Task<bool> IsFloorExists(int floorNumber);


        Task SaveChanges();
    }
}