using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.Core.Interfaces
{
    public interface IReservationRepository
    {
        Task<(IEnumerable<Reservation> Items, int TotalCount)> GetAllReservations(
           int? studentId = null,
           int? guestId = null,
           int? roomId = null,
           ReservationType? type = null,
           ReservationStatus? status = null,
           int pageIndex = 1,
           int pageSize = 10);

        Task<Reservation?> GetReservationById(int id);
        Task<Reservation?> GetActiveReservationByStudentId(int studentId);
        Task<Reservation?> GetActiveReservationByGuestId(int guestId);

        Task<Reservation> CreateReservation(Reservation reservation);
        Task<Reservation> UpdateReservation(Reservation reservation);

        Task<bool> HasActiveReservationForRoom(int roomId);
        Task SaveChanges();
    }
}