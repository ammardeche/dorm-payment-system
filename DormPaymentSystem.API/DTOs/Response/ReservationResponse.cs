using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public ReservationType Type { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal RateSnapshot { get; set; }
        public decimal TotalAmount { get; set; }

        public int RoomId { get; set; }
        public ReservationRoomResponse? Room { get; set; }

        public int? StudentId { get; set; }
        public ReservationStudentResponse? Student { get; set; }

        public int? GuestId { get; set; }
        public ReservationGuestResponse? Guest { get; set; }

        public int? InvitationId { get; set; }
        public ReservationInvitationResponse? Invitation { get; set; }

        public IEnumerable<ReservationPaymentResponse> Payments { get; set; } = new List<ReservationPaymentResponse>();

        public ReservationResponse(Reservation reservation)
        {
            Id = reservation.Id;
            Type = reservation.Type;
            Status = reservation.Status;
            CheckIn = reservation.CheckIn;
            CheckOut = reservation.CheckOut;
            RateSnapshot = reservation.RateSnapshot;
            TotalAmount = reservation.TotalAmount;

            RoomId = reservation.RoomId;
            Room = reservation.Room != null ? new ReservationRoomResponse
            {
                Id = reservation.Room.Id,
                RoomNumber = reservation.Room.RoomNumber
            } : null;

            // Only ONE of these three is set depending on Type
            if (reservation.Student != null)
            {
                Student = new ReservationStudentResponse(reservation.Student);
            }

            if (reservation.Guest != null)
            {
                Guest = new ReservationGuestResponse(reservation.Guest);
            }

            if (reservation.Invitation != null)
            {
                Invitation = new ReservationInvitationResponse(reservation.Invitation);
            }

            Payments = reservation.Payments.Select(p => new ReservationPaymentResponse
            {
                Id = p.Id,
                Amount = p.Amount,
                ReceiptNumber = p.ReceiptNumber,
                PaymentDate = p.PaymentDate,
                DueDate = p.DueDate,
                Status = p.Status,
                Method = p.Method
            });
        }
    }

    public class ReservationRoomResponse
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
    }

    public class ReservationStudentResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }

        public ReservationStudentResponse(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            StudentNumber = student.StudentNumber;
        }
    }

    public class ReservationGuestResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public ReservationGuestResponse(Guest guest)
        {
            Id = guest.Id;
            FullName = guest.FullName;
            NationalId = guest.NationalId;

        }
    }

    public class ReservationInvitationResponse
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public string GuestIdentityId { get; set; }

        public ReservationInvitationResponse(Invitation invitation)
        {
            Id = invitation.Id;

        }
    }

    public class ReservationPaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod? Method { get; set; }
    }
}
