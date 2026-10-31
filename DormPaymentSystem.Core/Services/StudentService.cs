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
        private readonly IReservationService _reservationService;

        public StudentService(
            IStudentRepository studentRepository,
            IReservationService reservationService
           )
        {
            _studentRepository = studentRepository;

            _reservationService = reservationService;
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
                UserId = userId
            };

            await _studentRepository.CreateStudent(student);


            return student;
        }

        public async Task<Student> UpdateStudentAsync(
            int id,
            string? firstName,
            string? lastName,
            string? email,
            string? phoneNumber
           )
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



            await _studentRepository.UpdateStudent(student);
            return student;
        }

        public async Task<bool> CanStudentLeaveAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentById(studentId);
    if (student == null)
        throw new AppNotFoundException($"Student with id '{studentId}' not found.");

    var hasUnpaidMonths = student.Reservations
        .Where(r => r.Type == ReservationType.StudentStay)
        .SelectMany(r => r.Payments)
        .Any(p => !p.IsDeleted && p.Status != PaymentStatus.Paid);

    return !hasUnpaidMonths;
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

            var activeReservation = await _reservationService.GetActiveReservationByStudentIdAsync(studentId);
            if (activeReservation != null)
                await _reservationService.CheckOutAsync(activeReservation.Id);
          

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

        public async Task<StudentStatsResponse> GetStudentStats()
        {
            return await _studentRepository.GetStudentStats();
        }

        public async Task<bool> ActivateStudentAsync(int studentId)
        {
           var student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{studentId}' not found.");

            if (student.IsActive)
                throw new AppValidationException("Student is already active.");

            student.IsActive = true;
            student.DepartureDate = null;
            student.DepartureNote = null;

            await _studentRepository.UpdateStudent(student);
            return true;
        }
    }

}

