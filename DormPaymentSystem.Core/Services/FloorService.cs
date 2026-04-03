using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepo;
        public FloorService(IFloorRepository floorRepo)
        {
            _floorRepo = floorRepo;
        }

        public async Task<Floor> CreateFloorAsync(int floorNumber, int totalRooms)

        {

            // need to check if floor number less than 0 

            if (floorNumber <= 0)
                throw new AppValidationException("Floor number must be greater than 0.");
            if (totalRooms <= 0)
            {
                throw new AppValidationException("floor must be have at least one room.");
            }

            var existingFloor = await IsFloorExistsAsync(floorNumber);

            if (existingFloor)
            {
                throw new AppConflictException($"Floor with number {floorNumber} already exists.");
            }

            var floor = new Floor
            {
                FloorNumber = floorNumber,
                TotalRooms = totalRooms
            };

            await _floorRepo.CreateFloor(floor);
            return floor;
        }
        public async Task<bool> DeleteFloorAsync(int floorNumber)
        {
            var existingFloor = await _floorRepo.GetFloorByFloorNumber(floorNumber);

            if (existingFloor == null)
            {
                throw new AppNotFoundException($"Floor with number {floorNumber} not found.");
            }

            await _floorRepo.DeleteFloor(existingFloor);
            return true;
        }


        public async Task<Floor> UpdateFloorAsync(int floorNumber, int totalRooms)
        {
            var existingFloor = await _floorRepo.GetFloorByFloorNumber(floorNumber);

            if (floorNumber <= 0)
            {
                throw new AppValidationException($" the floor number{floorNumber} must be greater than 0");
            }

            if (totalRooms <= 0)
            {
                throw new AppValidationException($" the floor number{totalRooms} must be greater than 0");
            }

            if (existingFloor == null)
            {
                throw new AppNotFoundException($"Floor with number {floorNumber} not found");
            }


            existingFloor.FloorNumber = floorNumber;
            existingFloor.TotalRooms = totalRooms;

            await _floorRepo.UpdateFloor(existingFloor);
            return existingFloor;
        }

        public async Task<bool> IsFloorExistsAsync(int floorNumber)
        {
            return await _floorRepo.IsFloorExists(floorNumber);
        }

        public async Task<IEnumerable<Floor>> GetAllFloorsAsync() => await _floorRepo.GetAllFloors();
    }
}