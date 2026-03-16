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
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task<Student> GetStudentByNumber(string studentNumber);

        // FILTER
        Task<IEnumerable<Student>> GetStudentsByRoom(int roomId);
        Task<IEnumerable<Student>> GetActiveStudents();

        // CREATE
        Task<Student> CreateStudent(Student student);

        // UPDATE
        Task<Student> UpdateStudent(Student student);

        // DELETE
        Task<bool> DeleteStudent(int id);

        // CHECK
        Task<bool> StudentExists(int id);
        Task<bool> StudentNumberExists(string studentNumber);

        // SAVE
        Task SaveChanges();
    }
}