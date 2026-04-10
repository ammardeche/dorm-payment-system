using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.API.Queries;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]
    [Route("api/invitations")]
    public class InvitationController : ControllerBase
    {

        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        // GET: api/invitations
        [HttpGet]
        public async Task<IActionResult> GetInvitations([FromQuery] InvitationQuery q)
        {
            var invitations = await _invitationService.GetInvitationsAsync(
                roomId: q.RoomId,
                studentId: q.StudentId,
                month: q.Month,
                year: q.Year
            );
            return Ok(invitations.Select(i => new InvitationResponse(i)));
        }

        // GET: api/invitations/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInvitationById(int id)
        {
            var invitations = await _invitationService.GetInvitationsAsync(id: id);
            var invitation = invitations.FirstOrDefault();
            if (invitation == null)
                return NotFound($"Invitation with id '{id}' not found.");

            return Ok(new InvitationResponse(invitation));
        }

        // POST: api/invitations
        [HttpPost]
        public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationRequest request)
        {
            var invitation = await _invitationService.CreateInvitationAsync(
                request.GuestName,
                request.GuestIdentityId,
                request.RoomId,
                request.InvitedByStudentId
            );
            return CreatedAtAction(nameof(GetInvitationById), new { id = invitation.Id }, new InvitationResponse(invitation));
        }

        // DELETE: api/invitations/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInvitation(int id)
        {
            await _invitationService.DeleteInvitationAsync(id);
            return NoContent();
        }

        // GET: api/invitations/can-invite/{roomId}
        [HttpGet("can-invite/{roomId:int}")]
        public async Task<IActionResult> CanRoomInvite(int roomId)
        {
            var canInvite = await _invitationService.CanRoomInviteThisMonthAsync(roomId);
            return Ok(new { roomId, canInvite });
        }
    }

}
