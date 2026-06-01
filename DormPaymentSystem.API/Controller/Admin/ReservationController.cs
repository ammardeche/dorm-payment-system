using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.API.Queries;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.API.Controller.Admin
{
    [ApiController]
    public class ReservationController : AdminControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] ReservationQuery q)
        {
            var (items, totalCount) = await _reservationService.GetAllReservationsAsync(
                q.StudentId, q.GuestId, q.RoomId,
                q.Type, q.Status, q.PageIndex, q.PageSize);

            var pagination = new Pagination(q.PageSize, q.PageIndex, totalCount);

            return Ok(new Response<IEnumerable<ReservationResponse>>(
                items.Select(r => new ReservationResponse(r)),
                pagination));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            return Ok(new Response<ReservationResponse>(new ReservationResponse(reservation)));
        }

        [HttpPost("student")]
        public async Task<IActionResult> CreateStudentReservation(
            [FromBody] CreateStudentReservationRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Invalid request.")));

            var reservation = await _reservationService.CreateStudentReservationAsync(
                req.StudentId,
                req.RoomId,
                req.CheckIn,
                req.Months,
                req.ReceivedByUserId);

            return Ok(new Response<ReservationResponse>(new ReservationResponse(reservation)));
        }

        [HttpPost("guest")]
        public async Task<IActionResult> CreateGuestReservation(
            [FromBody] CreateGuestReservationRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Invalid request.")));

            var reservation = await _reservationService.CreateGuestReservationAsync(
                req.GuestId,
                req.RoomId,
                req.CheckIn,
                req.CheckOut,
                req.ReceivedByUserId);

            return Ok(new Response<ReservationResponse>(new ReservationResponse(reservation)));
        }

        [HttpPost("invitation")]
        public async Task<IActionResult> CreateInvitationReservation(
            [FromBody] CreateInvitationReservationRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Invalid request.")));

            var reservation = await _reservationService.CreateInvitationReservationAsync(
                req.InvitationId,
                req.RoomId,
                req.CheckIn,
                req.CheckOut);

            return Ok(new Response<ReservationResponse>(new ReservationResponse(reservation)));
        }

        [HttpPost("checkout/{id}")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var reservation = await _reservationService.CheckOutAsync(id);
            return Ok(new Response<ReservationResponse>(new ReservationResponse(reservation)));
        }
    }
}
