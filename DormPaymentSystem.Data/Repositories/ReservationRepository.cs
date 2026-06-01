using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class ReservationRepository : IReservationRepository
    {

        private readonly DormPaymentDbContext _context;

        public ReservationRepository(DormPaymentDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Reservation> Items, int TotalCount)> GetAllReservations(
            int? studentId = null,
            int? guestId = null,
            int? roomId = null,
            ReservationType? type = null,
            ReservationStatus? status = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.Student)
                .Include(r => r.Guest)
                .Include(r => r.Invitation)
                .Include(r => r.Payments)
                .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(r => r.StudentId == studentId.Value);

            if (guestId.HasValue)
                query = query.Where(r => r.GuestId == guestId.Value);

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            if (type.HasValue)
                query = query.Where(r => r.Type == type.Value);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CheckIn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Reservation?> GetReservationById(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Student)
                .Include(r => r.Guest)
                .Include(r => r.Invitation)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation?> GetActiveReservationByStudentId(int studentId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r =>
                    r.StudentId == studentId &&
                    r.Status == ReservationStatus.Active);
        }

        public async Task<Reservation?> GetActiveReservationByGuestId(int guestId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r =>
                    r.GuestId == guestId &&
                    r.Status == ReservationStatus.Active);
        }

        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await SaveChanges();
            return reservation;
        }

        public async Task<Reservation> UpdateReservation(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await SaveChanges();
            return reservation;
        }

        public async Task<bool> HasActiveReservationForRoom(int roomId)
        {
            return await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.Status == ReservationStatus.Active);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

}
