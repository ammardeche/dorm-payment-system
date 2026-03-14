using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.Data.Interfaces
{
    public interface IStudentRepository
    {
        public Task<IEnumerable<Student>> getAllStudents();

        public Task<Student> GetStudentById(int id);
        public Task<Student> CreateStudent(Student student);

        public Task<Student> DeleteStudent(int id);
        public Task<Student> UpdateStudent(Student student);
    }
}