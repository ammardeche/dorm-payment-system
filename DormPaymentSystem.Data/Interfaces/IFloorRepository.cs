using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Repositories
{
    public interface IFloorRepository
    {
        public Task<IEnumerable<Floor>> GetAllFloors();
        public Task<Floor> GetFloorById(int Id);
        public Task<Floor> GetFloorByFloorNumber(int floorNumber);
        public Task<Floor> CreateFloor(Floor floor);
        public Task<Floor> UpdateFloor(Floor floor);
        public Task<Floor> DeleteFloor(int id);


        public Task SaveChanges();
    }
}