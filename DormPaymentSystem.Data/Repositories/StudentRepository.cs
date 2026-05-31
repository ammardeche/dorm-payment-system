using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Data;
using DormPaymentSystem.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DormPaymentDbContext _context;

        public StudentRepository(DormPaymentDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Student> Items, int TotalCount)> GetAllStudents(
            int? roomId = null,
            bool? isActive = null,
            string? studentNumber = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var query = _context.Students
                .AsNoTracking()
                .Include(s => s.Room)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Payments)
                .AsQueryable();

            if (roomId.HasValue)
                query = query.Where(s => s.RoomId == roomId.Value);

            if (isActive.HasValue)
                query = query.Where(s => s.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(studentNumber))
                query = query.Where(s => s.StudentNumber == studentNumber);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.LastName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _context.Students
                .Include(s => s.Room)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Payments)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetStudentByUserId(string userId)
        {
            return await _context.Students
                .Include(s => s.Room)
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.Payments)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<Student> CreateStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await SaveChanges();
            return student;
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            await SaveChanges();
            return student;
        }

        public async Task<bool> DeleteStudent(Student student)
        {
            _context.Students.Remove(student);
            await SaveChanges();
            return true;
        }

        public async Task<bool> StudentExists(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> StudentNumberExists(string studentNumber)
        {
            return await _context.Students.AnyAsync(s => s.StudentNumber == studentNumber);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

}
