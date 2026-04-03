using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IStudentService
    {
        // READ - simple pass through, no logic needed
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);          // throws if not found
        Task<Student?> GetStudentByNumberAsync(string studentNumber);
        Task<IEnumerable<Student>> GetStudentsByRoomAsync(int roomId);
        Task<IEnumerable<Student>> GetActiveStudentsAsync();

        // WRITE - has business logic
        Task<Student> CreateStudentAsync(
            string firstName,
            string lastName,
            string email,
            string studentNumber,
            string? phoneNumber,
            DateTime enrollmentDate,
            int roomId
          );   // check duplicate number
        Task<Student> UpdateStudentAsync(
             int id,
            string firstName,
            string lastName,
            string email,
            string? phoneNumber,
            int roomId
        );   // check exists
        Task<bool> CanStudentLeaveAsync(int studentId);  // checks all months paid
        Task<bool> DeactivateStudentAsync(int studentId, string? departureNote); // receptionist

        Task<bool> DeleteStudentAsync(int id);               // check can delete
    }
}