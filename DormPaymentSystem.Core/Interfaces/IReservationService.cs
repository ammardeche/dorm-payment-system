using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IReservationService
    {
        Task<(IEnumerable<Reservation> Items, int TotalCount)> GetAllReservationsAsync(
            int? studentId = null,
            int? guestId = null,
            int? roomId = null,
            ReservationType? type = null,
            ReservationStatus? status = null,
            int pageIndex = 1,
            int pageSize = 10);

        Task<Reservation?> GetReservationByIdAsync(int id);

        // Student checks in for a semester
        Task<Reservation> CreateStudentReservationAsync(
            int studentId,
            int roomId,
            DateTime checkIn,
            int months,               // how many months — 1, 2, 6 etc
            string receivedByUserId); // who processed it

        // External guest checks in
        Task<Reservation> CreateGuestReservationAsync(
            int guestId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut,
            string receivedByUserId);

        // Student invites a friend — free
        Task<Reservation> CreateInvitationReservationAsync(
            int invitationId,
            int roomId,
            DateTime checkIn,
            DateTime checkOut);

        // Guest or invited friend checks out
        Task<Reservation> CheckOutAsync(int reservationId);

        Task<Reservation?> GetActiveReservationByStudentIdAsync(int studentId);
    }
}