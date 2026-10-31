using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.Admin;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.API.Queries;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.API.Controller
{

    public class RoomController : AdminControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRooms([FromQuery] RoomQuery q)
        {
            var (items, totalCount) = await _roomService.GetAllRoomsAsync(
                q.Status, q.FloorNumber, q.PageIndex, q.PageSize);

            var pagination = new Pagination(q.PageSize, q.PageIndex, totalCount);

            return Ok(new Response<IEnumerable<RoomResponse>>(
                items.Select(r => new RoomResponse(r)),
                pagination));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            return Ok(new Response<RoomResponse>(new RoomResponse(room!)));
        }

        [HttpGet("is-available/{id}")]
        public async Task<IActionResult> IsRoomAvailable(int id)
        {
            var available = await _roomService.IsRoomAvailableAsync(id);
            return Ok(new Response<bool>(available));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Invalid request.")));

            var room = await _roomService.CreateRoomAsync(
                req.RoomNumber, req.Capacity, req.FloorNumber);

            return Ok(new Response<RoomResponse>(new RoomResponse(room)));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoom(
            int id, [FromBody] UpdateRoomRequest req)
        {
            var room = await _roomService.UpdateRoomAsync(
                id, req.RoomNumber, req.Capacity, req.Status);

            return Ok(new Response<RoomResponse>(new RoomResponse(room)));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _roomService.DeleteRoomAsync(id);
            return Ok(new Response<string>("Room removed successfully."));
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetRoomStats()
        {
            var stats = await _roomService.GetRoomStatsAsync();
            return Ok(new Response<RoomStatsResponse>(stats));
        }
    }

}
