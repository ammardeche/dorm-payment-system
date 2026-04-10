using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Interfaces;
using DormPaymentSystem.Data.Repositories;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.Core.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IStudentRepository _studentRepository;

        public InvitationService(
            IInvitationRepository invitationRepository,
            IRoomRepository roomRepository,
            IStudentRepository studentRepository)
        {
            _invitationRepository = invitationRepository;
            _roomRepository = roomRepository;
            _studentRepository = studentRepository;
        }

        public async Task<bool> CanRoomInviteThisMonthAsync(int roomId)
        {
            if (roomId <= 0)
                throw new AppValidationException("Room ID must be greater than zero.");

            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var hasInvitation = await _invitationRepository.RoomHasInvitationThisMonth(roomId, currentMonth, currentYear);
            return !hasInvitation;
        }

        public async Task<Invitation> CreateInvitationAsync(string guestName, string guestIdentityId, int roomId, int invitedByStudentId)
        {
            if (string.IsNullOrWhiteSpace(guestName))
                throw new AppValidationException("Guest name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(guestIdentityId))
                throw new AppValidationException("Guest identity ID cannot be null or empty.");
            if (roomId <= 0)
                throw new AppValidationException("Room ID must be greater than zero.");
            if (invitedByStudentId <= 0)
                throw new AppValidationException("Invited by student ID must be greater than zero.");

            // validate room exists
            var roomExists = await _roomRepository.RoomExists(roomId);
            if (!roomExists)
                throw new AppNotFoundException($"Room with id '{roomId}' not found.");

            // validate student exists
            var studentExists = await _studentRepository.StudentExists(invitedByStudentId);
            if (!studentExists)
                throw new AppNotFoundException($"Student with id '{invitedByStudentId}' not found.");

            // check one-per-month rule
            var canInvite = await CanRoomInviteThisMonthAsync(roomId);
            if (!canInvite)
                throw new AppConflictException("This room has already invited a guest this month.");

            var invitation = new Invitation
            {
                GuestName = guestName,
                GuestIdentityId = guestIdentityId,
                RoomId = roomId,
                InvitedByStudentId = invitedByStudentId,
                InvitationDate = DateTime.UtcNow,
                InvitationMonth = DateTime.UtcNow.Month,
                InvitationYear = DateTime.UtcNow.Year
            };

            return await _invitationRepository.CreateInvitation(invitation);
        }

        public async Task<bool> DeleteInvitationAsync(int id)
        {
            if (id <= 0)
                throw new AppValidationException("Invitation ID must be greater than zero.");

            var invitations = await _invitationRepository.GetInvitations(id: id);
            var invitation = invitations.FirstOrDefault();

            if (invitation == null)
                throw new AppNotFoundException($"Invitation with id '{id}' not found.");

            return await _invitationRepository.DeleteInvitation(invitation);
        }

        public async Task<IEnumerable<Invitation>> GetInvitationsAsync(int? id = null, int? roomId = null, int? studentId = null, int? month = null, int? year = null)
        {
            return await _invitationRepository.GetInvitations(
                id: id,
                roomId: roomId,
                studentId: studentId,
                month: month,
                year: year
            );
        }
    }
}