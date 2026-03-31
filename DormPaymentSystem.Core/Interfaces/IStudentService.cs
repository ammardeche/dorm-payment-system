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
        Task<Student> CreateStudentAsync(Student student);   // check duplicate number
        Task<Student> UpdateStudentAsync(Student student);   // check exists
        Task<bool> DeleteStudentAsync(int id);               // check can delete
    }
}