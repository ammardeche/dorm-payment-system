using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class InvitationResponse
    {
        public int Id { get; set; }
        public int InvitationMonth { get; set; }
        public int InvitationYear { get; set; }
        public DateTime InvitationDate { get; set; }
        public InvitationStatus Status { get; set; }

        public InvitationStudentResponse InvitedBy { get; set; } = null!;
        public InvitationGuestResponse Guest { get; set; } = null!;

        // null until guest checks in
        public int? ReservationId { get; set; }

        public InvitationResponse(Invitation invitation)
        {
            Id = invitation.Id;
            InvitationMonth = invitation.InvitationMonth;
            InvitationYear = invitation.InvitationYear;
            InvitationDate = invitation.InvitationDate;
            Status = invitation.Status;
            ReservationId = invitation.ReservationId;

            InvitedBy = new InvitationStudentResponse
            {
                Id = invitation.InvitedByStudentId,
                FullName = invitation.InvitedByStudent != null
                    ? $"{invitation.InvitedByStudent.FirstName} {invitation.InvitedByStudent.LastName}"
                    : string.Empty,
                StudentNumber = invitation.InvitedByStudent?.StudentNumber ?? string.Empty
            };

            Guest = new InvitationGuestResponse
            {
                Id = invitation.GuestId,
                FullName = invitation.Guest?.FullName ?? string.Empty,
                NationalId = invitation.Guest?.NationalId ?? string.Empty
            };
        }
    }

    public class InvitationStudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string StudentNumber { get; set; } = null!;
    }

    public class InvitationGuestResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
    }

}