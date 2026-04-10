using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;

namespace DormPaymentSystem.API.DTOs.Response
{
    public class InvitationResponse
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public string GuestIdentityId { get; set; }
        public int RoomId { get; set; }
        public int? RoomNumber { get; set; }
        public int InvitedByStudentId { get; set; }
        public string InvitedByStudentName { get; set; }
        public int InvitationMonth { get; set; }
        public int InvitationYear { get; set; }
        public DateTime InvitationDate { get; set; }

        public InvitationResponse(Invitation invitation)
        {
            Id = invitation.Id;
            GuestName = invitation.GuestName;
            GuestIdentityId = invitation.GuestIdentityId;
            RoomId = invitation.RoomId;
            RoomNumber = invitation.Room?.RoomNumber;
            InvitedByStudentId = invitation.InvitedByStudentId;
            InvitedByStudentName = invitation.InvitedByStudent?.FirstName;
            InvitationMonth = invitation.InvitationMonth;
            InvitationYear = invitation.InvitationYear;
            InvitationDate = invitation.InvitationDate;
        }
    }
}