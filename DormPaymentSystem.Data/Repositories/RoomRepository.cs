using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {

        private readonly DormPaymentDbContext _context;

        public RoomRepository(DormPaymentDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Room> Items, int TotalCount)> GetAllRooms(
            RoomStatus? status = null,
            int? floorNumber = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Rooms
                .AsNoTracking()
                .Include(r => r.Students)
                .Include(r => r.Reservations)
                    .ThenInclude(r => r.Guest)
                .Include(r => r.Invitations)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            // filter by FloorNumber int — no join needed
            if (floorNumber.HasValue)
                query = query.Where(r => r.FloorNumber == floorNumber.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.FloorNumber)
                .ThenBy(r => r.RoomNumber)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Room?> GetRoomById(int id)
        {
            return await _context.Rooms
                .Include(r => r.Students)
                .Include(r => r.Reservations)
                    .ThenInclude(r => r.Guest)
                .Include(r => r.Invitations)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room> CreateRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await SaveChanges();
            return room;
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            await SaveChanges();
            return room;
        }

        public async Task<bool> DeleteRoom(Room room)
        {
            _context.Rooms.Remove(room);
            await SaveChanges();
            return true;
        }

        public async Task<bool> RoomExists(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> RoomNumberExist(int roomNumber)
        {
            return await _context.Rooms.AnyAsync(r => r.RoomNumber == roomNumber);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<RoomStatsResponse> GetRoomStats()
        {
            var totalRooms = await _context.Rooms.CountAsync();
            var availableRooms = await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Available);
            var fullRooms = await _context.Rooms.CountAsync(r => r.Status == RoomStatus.Full);
            var freeBeds = await _context.Rooms.Where(r => r.Status != RoomStatus.Maintenance).SumAsync(r => r.Capacity - r.CurrentOccupancy); 

            return new RoomStatsResponse
            {
                Total = totalRooms,
                Available = availableRooms,
                Full = fullRooms,
                FreeBeds = freeBeds
            };
        }
    }
}