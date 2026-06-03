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
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuestRepository _guestRepository;

        public InvitationService(
            IInvitationRepository invitationRepository,
            IStudentRepository studentRepository,
            IGuestRepository guestRepository)
        {
            _invitationRepository = invitationRepository;
            _studentRepository = studentRepository;
            _guestRepository = guestRepository;
        }

        public async Task<(IEnumerable<Invitation> Items, int TotalCount)> GetInvitationsAsync(
            int? studentId = null,
            int? guestId = null,
            int? month = null,
            int? year = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            return await _invitationRepository.GetInvitations(
                studentId, guestId, month, year, pageIndex, pageSize);
        }

        public async Task<Invitation?> GetInvitationByIdAsync(int id)
        {
            var invitation = await _invitationRepository.GetInvitationById(id);
            if (invitation == null)
                throw new AppNotFoundException($"Invitation with id '{id}' not found.");
            return invitation;
        }

        public async Task<bool> CanStudentInviteThisMonthAsync(int studentId)
        {
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;

            // 1 invitation per student per month — not per room
            var hasInvitation = await _invitationRepository
                .StudentHasInvitationThisMonth(studentId, month, year);

            return !hasInvitation;
        }

        public async Task<Invitation> CreateInvitationAsync(
            int invitedByStudentId,
            int guestId)
        {
            // 1. validate student exists and is active
            var student = await _studentRepository.GetStudentById(invitedByStudentId);
            if (student == null)
                throw new AppNotFoundException(
                    $"Student with id '{invitedByStudentId}' not found.");

            if (!student.IsActive)
                throw new AppValidationException(
                    "Inactive student cannot create an invitation.");

            // 2. validate guest exists
            var guest = await _guestRepository.GetGuestById(guestId);
            if (guest == null)
                throw new AppNotFoundException(
                    $"Guest with id '{guestId}' not found.");

            // 3. enforce 1 invitation per student per month
            var canInvite = await CanStudentInviteThisMonthAsync(invitedByStudentId);
            if (!canInvite)
                throw new AppConflictException(
                    "Student has already used their invitation this month.");

            // 4. create invitation — status is Pending until guest checks in
            var invitation = new Invitation
            {
                InvitedByStudentId = invitedByStudentId,
                GuestId = guestId,
                InvitationDate = DateTime.UtcNow,
                InvitationMonth = DateTime.UtcNow.Month,
                InvitationYear = DateTime.UtcNow.Year,
                Status = InvitationStatus.Pending
            };

            return await _invitationRepository.CreateInvitation(invitation);
        }

        public async Task<bool> DeleteInvitationAsync(int id)
        {
            var invitation = await _invitationRepository.GetInvitationById(id);
            if (invitation == null)
                throw new AppNotFoundException($"Invitation with id '{id}' not found.");

            // cannot delete a used invitation — it has a reservation linked
            if (invitation.Status == InvitationStatus.Used)
                throw new AppValidationException(
                    "Cannot delete a used invitation — it has an active reservation.");

            return await _invitationRepository.DeleteInvitation(invitation);
        }
    }
}
