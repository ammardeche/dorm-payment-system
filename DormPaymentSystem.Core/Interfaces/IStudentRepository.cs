using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Interfaces
{
    public interface IStudentRepository
    {
        // READ
        Task<IEnumerable<Student>> GetAllStudents(int? roomId = null, bool? isActive = null, string? studentNumber = null);
        Task<Student?> GetStudentById(int id);

        // CREATE
        Task<Student> CreateStudent(Student student);

        // UPDATE
        Task<Student> UpdateStudent(Student student);

        // DELETE
        Task<bool> DeleteStudent(Student student);

        // CHECK
        Task<bool> StudentExists(int id);
        Task<bool> StudentNumberExists(string studentNumber);

        // SAVE
        Task SaveChanges();
    }
}