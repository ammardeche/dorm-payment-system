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

        public async Task<IEnumerable<Room>> GetAllRooms(int? floorId, RoomStatus? status)
        {
            var query = _context.Rooms
           .AsNoTracking()
           .Include(r => r.Students)
           .Include(r => r.Floor)
           .Include(r => r.Guests)
           .Include(r => r.Invitations)
           .AsQueryable();


            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status);
            }
            if (floorId.HasValue)
            {
                query = query.Where(r => r.FloorId == floorId);
            }
            return await query.ToListAsync();
        }
        public async Task<Room?> GetRoomById(int id)
        {
            return await _context.Rooms
                  .Include(r => r.Students)
                 .Include(r => r.Floor)
              .Include(r => r.Guests)
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

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RoomNumberExist(int roomNumber)
        {
            return await _context.Rooms.AnyAsync(r =>
                 r.RoomNumber == roomNumber
            );
        }
    }
}