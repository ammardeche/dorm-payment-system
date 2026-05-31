using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Interfaces
{
    public interface IStudentRepository
    {
        Task<(IEnumerable<Student> Items, int TotalCount)> GetAllStudents(
       int? roomId = null,
       bool? isActive = null,
       string? studentNumber = null,
       int pageIndex = 1,
       int pageSize = 10);

        Task<Student?> GetStudentById(int id);
        Task<Student?> GetStudentByUserId(string userId);

        Task<Student> CreateStudent(Student student);
        Task<Student> UpdateStudent(Student student);
        Task<bool> DeleteStudent(Student student);

        Task<bool> StudentExists(int id);
        Task<bool> StudentNumberExists(string studentNumber);

        Task SaveChanges();
    }
}