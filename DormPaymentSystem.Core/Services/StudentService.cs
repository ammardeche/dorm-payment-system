using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomService _roomService;
        public StudentService(IStudentRepository studentRepository, IRoomRepository roomRepository, IRoomService roomService)
        {
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
            _roomService = roomService;
        }


        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudents();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentById(id);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{id}' not found.");
            return student;
        }

        public async Task<Student?> GetStudentByNumberAsync(string studentNumber)
        {
            var student = await _studentRepository.GetStudentByNumber(studentNumber);
            if (student == null)
                throw new AppNotFoundException($"Student with number '{studentNumber}' not found.");
            return student;
        }

        public async Task<IEnumerable<Student>> GetStudentsByRoomAsync(int roomId)
        {
            return await _studentRepository.GetStudentsByRoom(roomId);
        }

        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            return await _studentRepository.GetActiveStudents();
        }

        public async Task<Student> CreateStudentAsync(
         string firstName,
         string lastName,
         string email,
         string studentNumber,
         string? phoneNumber,
         DateTime enrollmentDate,
         int roomId)
        {
            // 1. validate cheap things first
            if (string.IsNullOrWhiteSpace(studentNumber))
                throw new AppValidationException("Student number cannot be empty.");

            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email cannot be empty.");

            // 2. check duplicate student number
            if (await _studentRepository.StudentNumberExists(studentNumber))
                throw new AppValidationException($"Student number '{studentNumber}' already exists.");

            // 3. check room exists and block if full
            if (roomId != 0)
            {
                var room = await _roomRepository.GetRoomById(roomId);
                if (room == null)
                    throw new AppNotFoundException($"Room with id '{roomId}' not found.");

                var studentsInRoom = await _studentRepository.GetStudentsByRoom(roomId);
                var activeCount = studentsInRoom.Count(s => s.IsActive);

                if (activeCount >= room.Capacity)
                    throw new AppValidationException($"Room '{roomId}' is already full.");
            }

            // 4. create student
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                StudentNumber = studentNumber,
                PhoneNumber = phoneNumber,
                EnrollmentDate = enrollmentDate,
                IsActive = true,
                RoomId = roomId
            };

            await _studentRepository.CreateStudent(student);

            // 5. update room status AFTER student is added
            if (roomId != 0)
            {
                var room = await _roomRepository.GetRoomById(roomId);
                var updatedStudents = await _studentRepository.GetStudentsByRoom(roomId);
                var updatedCount = updatedStudents.Count(s => s.IsActive);

                room.Status = updatedCount >= room.Capacity
                    ? RoomStatus.Full
                    : RoomStatus.Available;

                await _roomRepository.UpdateRoom(room);
            }

            return student;
        }

        public async Task<Student> UpdateStudentAsync(
            int id,
            string firstName,
            string lastName,
            string email,
            string? phoneNumber,
            int roomId)
        {
            var student = await _studentRepository.GetStudentById(id);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{id}' not found.");

            if (!student.IsActive)
                throw new AppValidationException("Cannot update a deactivated student.");

            if (roomId != 0 && roomId != student.RoomId)
            {
                var room = await _roomRepository.GetRoomById(roomId);
                if (room == null)
                    throw new AppNotFoundException($"Room with id '{roomId}' not found.");

                var studentsInRoom = await _studentRepository.GetStudentsByRoom(roomId);
                if (studentsInRoom.Count() >= room.Capacity)
                    throw new AppValidationException($"Room '{roomId}' is already full.");
            }

            student.FirstName = firstName;
            student.LastName = lastName;
            student.Email = email;
            student.PhoneNumber = phoneNumber;
            student.RoomId = roomId;

            await _studentRepository.UpdateStudent(student);
            return student;
        }

        public async Task<bool> CanStudentLeaveAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{studentId}' not found.");

            var current = new DateTime(student.EnrollmentDate.Year,
                                       student.EnrollmentDate.Month, 1);
            var today = new DateTime(DateTime.UtcNow.Year,
                                     DateTime.UtcNow.Month, 1);

            while (current <= today)
            {
                var paymentForMonth = student.Payments.FirstOrDefault(p =>
                    p.PaymentYear == current.Year &&
                    p.PaymentMonth == current.Month &&
                    p.Status == PaymentStatus.Paid);

                if (paymentForMonth == null)
                    return false;

                current = current.AddMonths(1);
            }

            return true;
        }

        public async Task<bool> DeactivateStudentAsync(int studentId, string? departureNote)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{studentId}' not found.");

            if (!student.IsActive)
                throw new AppValidationException("Student is already deactivated.");

            var canLeave = await CanStudentLeaveAsync(studentId);
            if (!canLeave)
                throw new AppValidationException("Student has unpaid months and cannot leave.");

            student.IsActive = false;
            student.DepartureDate = DateTime.UtcNow;
            student.DepartureNote = departureNote;
            student.RoomId = null;

            await _studentRepository.UpdateStudent(student);
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{studentId}' not found.");

            if (student.IsActive)
                throw new AppValidationException("Student must be deactivated before deleting.");

            await _studentRepository.DeleteStudent(student);
            return true;
        }

    }
}
