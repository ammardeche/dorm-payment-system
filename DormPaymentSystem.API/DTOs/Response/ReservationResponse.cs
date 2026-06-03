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

        // room
        public ReservationRoomResponse? Room { get; set; }

        // only one of these three will be set depending on Type
        public ReservationStudentResponse? Student { get; set; }
        public ReservationGuestResponse? Guest { get; set; }
        public ReservationInvitationResponse? Invitation { get; set; }

        // payments linked to this reservation
        public IEnumerable<ReservationPaymentResponse> Payments { get; set; }

        public ReservationResponse(Reservation reservation)
        {
            Id = reservation.Id;
            Type = reservation.Type;
            Status = reservation.Status;
            CheckIn = reservation.CheckIn;
            CheckOut = reservation.CheckOut;
            RateSnapshot = reservation.RateSnapshot;
            TotalAmount = reservation.TotalAmount;

            Room = reservation.Room != null
                ? new ReservationRoomResponse
                {
                    Id = reservation.Room.Id,
                    RoomNumber = reservation.Room.RoomNumber,
                    FloorNumber = reservation.Room.FloorNumber
                }
                : null;

            Student = reservation.Student != null
                ? new ReservationStudentResponse
                {
                    Id = reservation.Student.Id,
                    FullName = $"{reservation.Student.FirstName} {reservation.Student.LastName}",
                    StudentNumber = reservation.Student.StudentNumber
                }
                : null;

            Guest = reservation.Guest != null
                ? new ReservationGuestResponse
                {
                    Id = reservation.Guest.Id,
                    FullName = reservation.Guest.FullName,
                    NationalId = reservation.Guest.NationalId
                }
                : null;

            Invitation = reservation.Invitation != null
                ? new ReservationInvitationResponse
                {
                    Id = reservation.Invitation.Id,
                    InvitationMonth = reservation.Invitation.InvitationMonth,
                    InvitationYear = reservation.Invitation.InvitationYear,
                    Status = reservation.Invitation.Status
                }
                : null;

            Payments = reservation.Payments.Select(p => new ReservationPaymentResponse
            {
                Id = p.Id,
                Amount = p.Amount,
                ReceiptNumber = p.ReceiptNumber,
                Status = p.Status,
                PaymentMonth = p.PaymentMonth,
                PaymentYear = p.PaymentYear,
                DueDate = p.DueDate
            });
        }
    }

    public class ReservationRoomResponse
    {
        public int Id { get; set; }
        public int? RoomNumber { get; set; }
        public int FloorNumber { get; set; }
    }

    public class ReservationStudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
    }

    public class ReservationGuestResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
    }

    public class ReservationInvitationResponse
    {
        public int Id { get; set; }
        public int InvitationMonth { get; set; }
        public int InvitationYear { get; set; }
        public InvitationStatus Status { get; set; }
    }

    public class ReservationPaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ReceiptNumber { get; set; } = null!;
        public PaymentStatus Status { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public DateTime DueDate { get; set; }
    }

}

