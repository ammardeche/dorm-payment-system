using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]
    [Route("api/floors")]
    public class FloorController : ControllerBase
    {

        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> getAllFloors()
        {
            var floors = await _floorService.GetAllFloorsAsync();

            return Ok(floors.Select(f => new FloorResponse(f)));
        }

        [HttpPost("create")]

        public async Task<IActionResult> CreateFloor([FromBody] CreateFloorRequest req)
        {
            var floor = await _floorService.CreateFloorAsync(req.FloorNumber, req.TotalRooms);
            return Ok(new FloorResponse(floor));
        }

        [HttpDelete("delete/{floorNumber}")]
        public async Task<IActionResult> DeleteFloor([FromRoute] int floorNumber)
        {
            await _floorService.DeleteFloorAsync(floorNumber);


            return Ok($"Floor with number {floorNumber} deleted successfully.");
        }

        [HttpPut("update/{floorNumber}")]

        public async Task<IActionResult> UpdateFloor([FromRoute] int floorNumber, [FromBody] UpdateFloorRequest req)
        {
            var updatedFloor = await _floorService.UpdateFloorAsync(floorNumber, req.TotalRooms);
            return Ok(new FloorResponse(updatedFloor));
        }
    
    }

}