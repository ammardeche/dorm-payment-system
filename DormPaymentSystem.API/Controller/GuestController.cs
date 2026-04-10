using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]
    [Route("api/guests")]
    public class GuestController : ControllerBase
    {

        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGuests(
           [FromQuery] int? roomId = null,
           [FromQuery] string? nationalId = null,
           [FromQuery] bool? isActive = null)
        {
            var guests = await _guestService.GetAllGuestsAsync(roomId, nationalId, isActive);
            var response = guests.Select(g => new GuestResponse(g));
            return Ok(response);
        }

        // GET api/guests/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuestById(int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);
            return Ok(new GuestResponse(guest));
        }

        // POST api/guests/checkin
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInRequest request)
        {
            var existingGuest = await _guestService.CheckGuestByNationalIdAsync(request.NationalId);
            var guest = await _guestService.CheckInGuestAsync(
                request.FullName,
                request.NationalId,
                request.RoomId,
                request.RatePerNight,
                request.NightsStayed
            );
            return CreatedAtAction(nameof(GetGuestById), new { id = guest.Id }, new GuestResponse(guest));
        }

        // POST api/guests/{id}/checkout
        [HttpPost("{id}/checkout")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var guest = await _guestService.CheckOutGuestAsync(id);
            return Ok(new GuestResponse(guest));
        }
    }
}
