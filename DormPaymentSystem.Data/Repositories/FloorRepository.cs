using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class FloorRepository : IFloorRepository
    {

        private readonly DormPaymentDbContext _context;

        public FloorRepository(DormPaymentDbContext context)
        {
            _context = context;
        }
        // Create floor 
        public async Task<Floor> CreateFloor(Floor floor)
        {
            await _context.Floors.AddAsync(floor);
            await SaveChanges();
            return floor;
        }
        // Delete floor 
        public async Task<bool> DeleteFloor(Floor floor)
        {
            _context.Floors.Remove(floor);
            await SaveChanges();
            return true;
        }

        // Get all floors 
        public async Task<IEnumerable<Floor>> GetAllFloors()
        {
            var floors = await _context.Floors.Include(f => f.Rooms).ToListAsync();
            return floors;
        }

        //  Get floor by floor number
        public async Task<Floor> GetFloorByFloorNumber(int floorNumber) => await _context.Floors.FirstOrDefaultAsync(f => f.FloorNumber == floorNumber);

        public async Task<Floor> GetFloorById(int Id) => await _context.Floors.FindAsync(Id);

        public async Task<Floor> UpdateFloor(Floor floor)
        {
            _context.Floors.Update(floor);
            await SaveChanges();

            return floor;
        }

        public Task<bool> IsFloorExists(int floorNumber)
        {
            return _context.Floors.AnyAsync(f => f.FloorNumber == floorNumber);
        }
        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}