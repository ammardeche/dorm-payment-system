using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class MyInvitationResponse
    {
        public int Id { get; set; }
        public int InvitationMonth { get; set; }
        public int InvitationYear { get; set; }
        public DateTime InvitationDate { get; set; }
        public InvitationStatus Status { get; set; }
        public int? ReservationId { get; set; }
        public string GuestName { get; set; } = null!;
        public string GuestDocument { get; set; } = null!;   // *****45

        // guest is optional: after create we already have it in the controller
        public MyInvitationResponse(Invitation invitation = null!, Guest? guest = null)
        {
            var g = guest ?? invitation.Guest;

            Id = invitation.Id;
            InvitationMonth = invitation.InvitationMonth;
            InvitationYear = invitation.InvitationYear;
            InvitationDate = invitation.InvitationDate;
            Status = invitation.Status;
            ReservationId = invitation.ReservationId;
            GuestName = g?.FullName ?? string.Empty;
            GuestDocument = Mask(g?.DocumentNumber);
        }

        private static string Mask(string? number)
        {
            if (string.IsNullOrEmpty(number)) return string.Empty;
            if (number.Length < 6) return new string('*', number.Length); // too short to show anything
            return "*****" + number[^2..];                                // fixed 5 stars + last 2
        }
    }
}