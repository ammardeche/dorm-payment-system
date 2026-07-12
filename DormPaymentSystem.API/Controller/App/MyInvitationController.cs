using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.AuthController;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.API.Controller.App
{

    public class MyInvitationController : AppControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly IGuestService _guestService;

        public MyInvitationController(
            IInvitationService invitationService,
            IGuestService guestService)
        {
            _invitationService = invitationService;
            _guestService = guestService;
        }

        // GET api/app/myinvitation
        // student sees all their invitations
        [HttpGet]
        public async Task<IActionResult> GetMyInvitations(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var studentId = GetCurrentStudentId();

            var (items, totalCount) = await _invitationService
                .GetInvitationsAsync(
                    studentId: studentId,
                    pageIndex: pageIndex,
                    pageSize: pageSize);

            var pagination = new Pagination(pageSize, pageIndex, totalCount);

            return Ok(new Response<IEnumerable<InvitationResponse>>(
                items.Select(i => new InvitationResponse(i)),
                pagination));
        }

        // GET api/app/myinvitation/can-invite
        // student checks if they can invite someone this month
        [HttpGet("can-invite")]
        public async Task<IActionResult> CanInviteThisMonth()
        {
            var studentId = GetCurrentStudentId();
            var canInvite = await _invitationService
                .CanStudentInviteThisMonthAsync(studentId);

            return Ok(new Response<CanInviteResponse>(new CanInviteResponse
            {
                CanInvite = canInvite,
                Message = canInvite
                    ? "You can invite a guest this month."
                    : "You have already used your invitation this month."
            }));
        }

        // POST api/app/myinvitation/create
        // student creates invitation for their friend
        [HttpPost("create")]
        public async Task<IActionResult> CreateInvitation(
            [FromBody] AppCreateInvitationRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(
                    new AppValidationException("Invalid request.")));

            var studentId = GetCurrentStudentId();

            // register guest first if not exists
            var guest = await _guestService.RegisterGuestAsync(
                req.GuestFullName,
                req.GuestNationalId);

            // create invitation
            var invitation = await _invitationService.CreateInvitationAsync(
                studentId,
                guest.Id);

            return Ok(new Response<InvitationResponse>(
                new InvitationResponse(invitation)));
        }
    }
}
