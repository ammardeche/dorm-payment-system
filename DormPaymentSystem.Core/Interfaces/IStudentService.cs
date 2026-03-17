using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IStudentService
    {
        // BUSINESS OPERATIONS (Keep in Service)
        Task<Student> CreateStudent(Student student);
        Task<Student> UpdateStudent(Student student);
        Task<bool> DeleteStudent(int id);

        // VALIDATION HELPERS (Keep in Service)
        Task<bool> CanDeleteStudent(int id);

    }
}