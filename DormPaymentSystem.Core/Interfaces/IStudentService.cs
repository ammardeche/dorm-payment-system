using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IStudentService
    {
        Task<(IEnumerable<Student> Items, int TotalCount)> GetAllStudents(
       int? roomId = null,
       bool? isActive = null,
       string? studentNumber = null,
       int pageIndex = 1,
       int pageSize = 10);

        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentByUserIdAsync(string userId);

        Task<Student> CreateStudentAsync(
            string firstName,
            string lastName,
            string email,
            string studentNumber,
            string? phoneNumber,
            int roomId,
            string userId);

        Task<Student> UpdateStudentAsync(
            int id,
            string? firstName,
            string? lastName,
            string? email,
            string? phoneNumber,
            int? roomId);

        Task<bool> CanStudentLeaveAsync(int studentId);
        Task<bool> DeactivateStudentAsync(int studentId, string? departureNote);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}