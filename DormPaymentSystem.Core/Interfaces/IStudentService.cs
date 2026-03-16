using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IStudentService
    {
        // READ - What controllers/other services need
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task<Student> GetStudentByNumber(string studentNumber);

        // CREATE
        Task<Student> CreateStudent(Student student);

        // UPDATE
        Task<Student> UpdateStudent(Student student);

        // DELETE
        Task<bool> DeleteStudent(int id);

        // VALIDATION HELPERS
        Task<bool> StudentExists(int id);
        Task<bool> StudentNumberExists(string studentNumber);
        Task<bool> CanDeleteStudent(int id);  // Check if has unpaid payments

    }
}