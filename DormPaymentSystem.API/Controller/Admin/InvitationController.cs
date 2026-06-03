using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.Admin;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.API.Queries;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]

    public class InvitationController : AdminControllerBase
    {


        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] InvitationQuery q)
        {
            var (items, totalCount) = await _invitationService.GetInvitationsAsync(
                q.StudentId, q.GuestId, q.Month, q.Year, q.PageIndex, q.PageSize);

            var pagination = new Pagination(q.PageSize, q.PageIndex, totalCount);

            return Ok(new Response<IEnumerable<InvitationResponse>>(
                items.Select(i => new InvitationResponse(i)),
                pagination));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invitation = await _invitationService.GetInvitationByIdAsync(id);
            return Ok(new Response<InvitationResponse>(new InvitationResponse(invitation)));
        }

        [HttpGet("can-invite/{studentId}")]
        public async Task<IActionResult> CanStudentInvite(int studentId)
        {
            var canInvite = await _invitationService.CanStudentInviteThisMonthAsync(studentId);
            return Ok(new Response<bool>(canInvite));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateInvitationRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Invalid request.")));

            var invitation = await _invitationService.CreateInvitationAsync(
                req.InvitedByStudentId,
                req.GuestId);

            return Ok(new Response<InvitationResponse>(new InvitationResponse(invitation)));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _invitationService.DeleteInvitationAsync(id);
            return Ok(new Response<string>("Invitation deleted successfully."));
        }
    }


}

