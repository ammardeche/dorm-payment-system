using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.AuthController;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.App
{

    public class MyReservationController : AppControllerBase
    {

        private readonly IReservationService _reservationService;

        public MyReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetMyReservations()
        {
            var studentId = GetCurrentStudentId();

            var reservations = await _reservationService.GetActiveReservationByStudentIdAsync(studentId);

            if (reservations == null)
            {
                return NotFound(new Response<string>("No active reservations found for the student."));
            }

            return Ok(new Response<ReservationResponse>(new ReservationResponse(reservations)));
        }

        [HttpGet("history")]

        public async Task<IActionResult> GetMyHistory(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10
        )
        {

            var studentId = GetCurrentStudentId();

            var (items, totalCount) =
             await _reservationService.GetAllReservationsAsync(
                studentId,
                pageIndex,
                pageSize
             );

            var pagination = new Pagination(pageIndex, pageSize, totalCount);

            return Ok(new Response<IEnumerable<ReservationResponse>>(items.Select(r => new ReservationResponse(r)).ToList(), pagination));

        }

    }
}