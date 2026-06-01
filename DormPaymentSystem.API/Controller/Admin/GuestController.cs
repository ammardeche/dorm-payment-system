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
    public class GuestController : AdminControllerBase
    {


        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllGuests([FromQuery] GuestQuery q)
        {
            var (items, totalCount) = await _guestService.GetAllGuestsAsync(
                q.NationalId, q.FullName, q.PageIndex, q.PageSize);

            var pagination = new Pagination(q.PageSize, q.PageIndex, totalCount);

            return Ok(new Response<IEnumerable<GuestResponse>>(
                items.Select(g => new GuestResponse(g)),
                pagination));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuestById(int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);
            return Ok(new Response<GuestResponse>(new GuestResponse(guest)));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterGuest([FromBody] RegisterGuestRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Fill all required fields.")));

            var guest = await _guestService.RegisterGuestAsync(req.FullName, req.NationalId);
            return Ok(new Response<GuestResponse>(new GuestResponse(guest)));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateGuest(int id, [FromBody] UpdateGuestRequest req)
        {
            var guest = await _guestService.UpdateGuestAsync(id, req.FullName, req.NationalId);
            return Ok(new Response<GuestResponse>(new GuestResponse(guest)));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            await _guestService.DeleteGuestAsync(id);
            return Ok(new Response<string>("Guest removed successfully."));
        }
    }

}



