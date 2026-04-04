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
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {

        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/rooms/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRooms([FromQuery] RoomQuery q)
        {
            var rooms = await _roomService.GetAllRooms(q.Status, q.FloorId);
            return Ok(rooms.Select(r => new RoomResponse(r)));
        }

        // GET: api/rooms/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomById(id);
            return Ok(new RoomResponse(room!));
        }

        // POST: api/rooms/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest req)
        {
            var room = await _roomService.CreateRoom(req.RoomNumber, req.Capacity, req.FloorId);
            return Ok("room created successfully");
        }

        // PUT: api/rooms/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomRequest req)
        {
            var room = await _roomService.UpdateRoom(id, req.RoomNumber, req.Capacity, req.Status);
            return Ok("room created successfully");
        }

        // DELETE: api/rooms/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomService.DeleteRoom(id);

            return Ok("the room removed successfully ");
        }

        // GET: api/rooms/is-available/{roomId}
        [HttpGet("is-available/{roomId}")]
        public async Task<IActionResult> IsRoomAvailable(int roomId)
        {
            var room = await _roomService.IsRoomAvailable(roomId);
            return Ok(room);
        }



    }
}