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

        public StudentService(
            IStudentRepository studentRepository,
            IRoomRepository roomRepository)
        {
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
        }

        public async Task<(IEnumerable<Student> Items, int TotalCount)> GetAllStudentsAsync(
            int? roomId = null,
            bool? isActive = null,
            string? studentNumber = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _studentRepository.GetAllStudents(
                roomId, isActive, studentNumber, pageIndex, pageSize);
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentById(id);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{id}' not found.");
            return student;
        }

        public async Task<Student?> GetStudentByUserIdAsync(string userId)
        {
            var student = await _studentRepository.GetStudentByUserId(userId);
            if (student == null)
                throw new AppNotFoundException("Student profile not found.");
            return student;
        }

        public async Task<Student> CreateStudentAsync(
            string firstName,
            string lastName,
            string email,
            string studentNumber,
            string? phoneNumber,
            int? roomId,
            string userId)          // NEW — link to Identity account
        {
            if (string.IsNullOrWhiteSpace(studentNumber))
                throw new AppValidationException("Student number cannot be empty.");

            if (string.IsNullOrWhiteSpace(email))
                throw new AppValidationException("Email cannot be empty.");

            if (string.IsNullOrWhiteSpace(userId))
                throw new AppValidationException("UserId cannot be empty.");

            if (await _studentRepository.StudentNumberExists(studentNumber))
                throw new AppValidationException($"Student number '{studentNumber}' already exists.");

            // FIX 1 — unpack tuple correctly
            if (roomId.HasValue)
            {
                var room = await _roomRepository.GetRoomById(roomId.Value);
                if (room == null)
                    throw new AppNotFoundException($"Room with id '{roomId}' not found.");

                var (studentsInRoom, _) = await _studentRepository.GetAllStudents(
                    roomId: roomId.Value);
                var activeCount = studentsInRoom.Count(s => s.IsActive);

                if (activeCount >= room.Capacity)
                    throw new AppValidationException($"Room '{roomId}' is already full.");
            }

            // FIX 2 — include UserId
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                StudentNumber = studentNumber,
                PhoneNumber = phoneNumber,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true,
                RoomId = roomId,
                UserId = userId
            };

            await _studentRepository.CreateStudent(student);

            // update room occupancy after student is added
            if (roomId.HasValue)
            {
                var room = await _roomRepository.GetRoomById(roomId.Value);
                var (updatedStudents, _) = await _studentRepository.GetAllStudents(
                    roomId: roomId.Value);
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
            string? firstName,
            string? lastName,
            string? email,
            string? phoneNumber,
            int? roomId)
        {
            var student = await _studentRepository.GetStudentById(id);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{id}' not found.");

            if (!student.IsActive)
                throw new AppValidationException("Cannot update a deactivated student.");

            // FIX 3 — only update if value was actually passed, never wipe with null
            if (!string.IsNullOrWhiteSpace(firstName)) student.FirstName = firstName;
            if (!string.IsNullOrWhiteSpace(lastName)) student.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(email)) student.Email = email;
            if (!string.IsNullOrWhiteSpace(phoneNumber)) student.PhoneNumber = phoneNumber;

            if (roomId.HasValue && roomId.Value != student.RoomId)
            {
                var room = await _roomRepository.GetRoomById(roomId.Value);
                if (room == null)
                    throw new AppNotFoundException($"Room with id '{roomId}' not found.");

                var (studentsInRoom, _) = await _studentRepository.GetAllStudents(
                    roomId: roomId.Value);
                var activeCount = studentsInRoom.Count(s => s.IsActive);

                if (activeCount >= room.Capacity)
                    throw new AppValidationException($"Room '{roomId}' is already full.");

                student.RoomId = roomId.Value;
            }

            await _studentRepository.UpdateStudent(student);
            return student;
        }

        public async Task<bool> CanStudentLeaveAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{studentId}' not found.");

            // FIX 4 — go through Reservations → Payments
            // student.Payments no longer exists
            var allPayments = student.Reservations
                .Where(r => r.Type == ReservationType.StudentStay)
                .SelectMany(r => r.Payments)
                .ToList();

            var current = new DateTime(
                student.EnrollmentDate.Year,
                student.EnrollmentDate.Month, 1);

            var today = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month, 1);

            while (current <= today)
            {
                var paid = allPayments.Any(p =>
                    p.PaymentYear == current.Year &&
                    p.PaymentMonth == current.Month &&
                    p.Status == PaymentStatus.Paid);

                if (!paid) return false;

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

