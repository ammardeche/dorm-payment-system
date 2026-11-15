using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces.Building;
using DormPaymentSystem.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.Admin
{
    [Controller]
    [Route("api/user")]
    public class BuildingController : AdminControllerBase
    {

        private readonly IBuildingMemberShipService _membershipService;
        public BuildingController(IBuildingMemberShipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet("{id}/buildings")]
        public async Task<IActionResult> GetBuildings(string id)
        {
            var buildings = await _membershipService.GetBuildingsOfUser(id);
            return Ok(buildings.Select(b => new { b.Id, b.Name }));
        }

        [HttpPost("{id}/buildings/{buildingId}")]
        public async Task<IActionResult> AssignBuilding(string id, int buildingId)
        {
            await _membershipService.AssignAsync(id, buildingId);
            return Ok(new Response<string>("Building assigned."));
        }

        [HttpDelete("{id}/buildings/{buildingId}")]
        public async Task<IActionResult> RemoveBuilding(string id, int buildingId)
        {
            await _membershipService.RemoveAsync(id, buildingId);
            return NoContent();
        }

    }
}