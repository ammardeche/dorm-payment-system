using System;
using System.Collections.Generic;
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
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IDormitorySettingsRepository _settingsRepository;
        private readonly IPaymentRepository _paymentRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IStudentRepository studentRepository,
            IGuestRepository guestRepository,
            IRoomRepository roomRepository,
            IInvitationRepository invitationRepository,
            IDormitorySettingsRepository settingsRepository,
            IPaymentRepository paymentRepository)
        {
            _reservationRepository = reservationRepository;
            _studentRepository = studentRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _invitationRepository = invitationRepository;
            _settingsRepository = settingsRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<(IEnumerable<Reservation> Items, int TotalCount)> GetAllReservationsAsync(
            int? studentId = null,
            int? guestId = null,
            int? roomId = null,
            ReservationType? type = null,
            ReservationStatus? status = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _reservationRepository.GetAllReservations(
                studentId, guestId, roomId, type, status, pageIndex, pageSize);
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation == null)
                throw new AppNotFoundException($"Reservation with id '{id}' not found.");
            return reservation;
        }

        public async Task<Reservation?> GetActiveReservationByStudentIdAsync(int studentId)
        {
            return await _reservationRepository.GetActiveReservationByStudentId(studentId);
        }

        public async Task<Reservation> CreateStudentReservationAsync(
            int studentId,
            int roomId,
            DateTime checkIn,
            int months,
            string receivedByUserId)
        {
            // 1. validate student
            var student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new AppNotFoundException($"Student with id '{studentId}' not found.");

            if (!student.IsActive)
                throw new AppValidationException("Cannot create reservation for inactive student.");

            // 2. check student has no active reservation already
            var existing = await _reservationRepository
                .GetActiveReservationByStudentId(studentId);
            if (existing != null)
                throw new AppConflictException("Student already has an active reservation.");

            // 3. validate room
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            if (room.CurrentOccupancy >= room.Capacity)
                throw new AppValidationException("Room is full.");

            // 4. validate months
            if (months <= 0)
                throw new AppValidationException("Months must be greater than 0.");

            // 5. get current pricing
            var settings = await _settingsRepository.GetCurrentSettingsAsync();
            if (settings == null)
                throw new AppValidationException(
                    "Dormitory pricing is not configured. Contact admin.");

            // 6. create reservation
            var checkOut = checkIn.AddMonths(months);

            var reservation = new Reservation
            {
                Type = ReservationType.StudentStay,
                Status = ReservationStatus.Active,
                StudentId = studentId,
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                RateSnapshot = settings.StudentMonthlyRate,  // snapshot price
                TotalAmount = settings.StudentMonthlyRate * months
            };

            await _reservationRepository.CreateReservation(reservation);

            // 7. create one payment per month
            for (int i = 0; i < months; i++)
            {
                var paymentDate = checkIn.AddMonths(i);
                var payment = new Payment
                {
                    ReservationId = reservation.Id,
                    Amount = settings.StudentMonthlyRate,
                    PaymentMonth = paymentDate.Month,
                    PaymentYear = paymentDate.Year,
                    DueDate = new DateTime(paymentDate.Year, paymentDate.Month, 1),
                    Status = PaymentStatus.Pending,
                    ReceivedById = receivedByUserId,
                    ReceiptNumber = GenerateReceiptNumber("STD", studentId, i),
                    CreatedAt = DateTime.UtcNow
                };
                await _paymentRepository.CreatePayment(payment);
            }

            // 8. update room occupancy
            room.CurrentOccupancy += 1;
            if (room.CurrentOccupancy >= room.Capacity)
                room.Status = RoomStatus.Full;
            await _roomRepository.UpdateRoom(room);

            // 9. update student room
            student.RoomId = roomId;
            await _studentRepository.UpdateStudent(student);

            return reservation;
        }

        public async Task<Reservation> CreateGuestReservationAsync(
            int guestId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut,
            string receivedByUserId)
        {
            // 1. validate guest
            var guest = await _guestRepository.GetGuestById(guestId);
            if (guest == null)
                throw new AppNotFoundException($"Guest with id '{guestId}' not found.");

            // 2. check guest has no active reservation
            var existing = await _reservationRepository
                .GetActiveReservationByGuestId(guestId);
            if (existing != null)
                throw new AppConflictException("Guest already has an active reservation.");

            // 3. validate dates
            if (checkOut <= checkIn)
                throw new AppValidationException("Check-out must be after check-in.");

            // 4. validate room
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            if (room.CurrentOccupancy >= room.Capacity)
                throw new AppValidationException("Room is full.");

            // 5. get pricing
            var settings = await _settingsRepository.GetCurrentSettingsAsync();
            if (settings == null)
                throw new AppValidationException(
                    "Dormitory pricing is not configured. Contact admin.");

            // 6. calculate nights and total
            var nights = (int)Math.Ceiling((checkOut - checkIn).TotalDays);
            if (nights < 1) nights = 1;

            var reservation = new Reservation
            {
                Type = ReservationType.GuestStay,
                Status = ReservationStatus.Active,
                GuestId = guestId,
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                RateSnapshot = settings.GuestRatePerNight,
                TotalAmount = settings.GuestRatePerNight * nights
            };

            await _reservationRepository.CreateReservation(reservation);

            // 7. create single payment for full stay
            var payment = new Payment
            {
                ReservationId = reservation.Id,
                Amount = reservation.TotalAmount,
                PaymentMonth = checkIn.Month,
                PaymentYear = checkIn.Year,
                DueDate = checkIn,
                Status = PaymentStatus.Pending,
                ReceivedById = receivedByUserId,
                ReceiptNumber = GenerateReceiptNumber("GST", guestId, 0),
                CreatedAt = DateTime.UtcNow
            };
            await _paymentRepository.CreatePayment(payment);

            // 8. update room occupancy
            room.CurrentOccupancy += 1;
            if (room.CurrentOccupancy >= room.Capacity)
                room.Status = RoomStatus.Full;
            await _roomRepository.UpdateRoom(room);

            return reservation;
        }

        public async Task<Reservation> CreateInvitationReservationAsync(
            int invitationId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut)
        {
            // 1. validate invitation
            var invitation = await _invitationRepository.GetInvitationById(invitationId);
            if (invitation == null)
                throw new AppNotFoundException($"Invitation with id '{invitationId}' not found.");

            if (invitation.Status != InvitationStatus.Pending)
                throw new AppValidationException("Invitation has already been used or expired.");

            // 2. validate dates
            if (checkOut <= checkIn)
                throw new AppValidationException("Check-out must be after check-in.");

            // 3. validate room
            var room = await _roomRepository.GetRoomById(roomId);
            if (room == null)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            if (room.CurrentOccupancy >= room.Capacity)
                throw new AppValidationException("Room is full.");

            // 4. create reservation — always free
            var reservation = new Reservation
            {
                Type = ReservationType.InvitationStay,
                Status = ReservationStatus.Active,
                GuestId = invitation.GuestId,    // guest linked via invitation
                InvitationId = invitationId,
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                RateSnapshot = 0,                // free
                TotalAmount = 0                  // no payment created
            };

            await _reservationRepository.CreateReservation(reservation);

            // 5. mark invitation as used
            invitation.Status = InvitationStatus.Used;
            invitation.ReservationId = reservation.Id;
            await _invitationRepository.UpdateInvitation(invitation);

            // 6. update room occupancy
            room.CurrentOccupancy += 1;
            if (room.CurrentOccupancy >= room.Capacity)
                room.Status = RoomStatus.Full;
            await _roomRepository.UpdateRoom(room);

            return reservation;
        }

        public async Task<Reservation> CheckOutAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationById(reservationId);
            if (reservation == null)
                throw new AppNotFoundException($"Reservation with id '{reservationId}' not found.");

            if (reservation.Status != ReservationStatus.Active)
                throw new AppValidationException("Reservation is not active.");

            // set checkout
            reservation.Status = ReservationStatus.CheckedOut;
            reservation.CheckOut = DateTime.UtcNow;

            await _reservationRepository.UpdateReservation(reservation);

            // update room occupancy
            var room = await _roomRepository.GetRoomById(reservation.RoomId);
            if (room != null)
            {
                room.CurrentOccupancy = Math.Max(0, room.CurrentOccupancy - 1);
                room.Status = room.CurrentOccupancy < room.Capacity
                    ? RoomStatus.Available
                    : RoomStatus.Full;
                await _roomRepository.UpdateRoom(room);
            }

            return reservation;
        }

        // ── HELPERS ──────────────────────────────────────────────────
        private static string GenerateReceiptNumber(string prefix, int entityId, int index)
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = new Random().Next(1000, 9999);
            return $"RCPT-{datePart}-{prefix}{entityId}-{index}-{randomPart}";
        }
    }
}
