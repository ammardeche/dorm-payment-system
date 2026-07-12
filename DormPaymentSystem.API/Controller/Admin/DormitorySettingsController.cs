using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.Admin
{
    public class DormitorySettingsController : AdminControllerBase
    {
        private readonly IDormitorySettingsService _service;

        public DormitorySettingsController(
            IDormitorySettingsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var settings =
                await _service.GetCurrentSettingsAsync();

            return Ok(
                new Response<DormitorySettingsResponse>(
                    new DormitorySettingsResponse(settings)));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            CreateDormitorySettingsRequest req)
        {
            var settings =
                await _service.CreateSettingsAsync(
                    req.GuestRatePerNight,
                    req.StudentMonthlyRate);

            return Ok(
                new Response<DormitorySettingsResponse>(
                    new DormitorySettingsResponse(settings)));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(
            UpdateDormitorySettingsRequest req)
        {
            var settings =
                await _service.UpdateSettingsAsync(
                    req.GuestRatePerNight,
                    req.StudentMonthlyRate);

            return Ok(
                new Response<DormitorySettingsResponse>(
                    new DormitorySettingsResponse(settings)));
        }

    }
}