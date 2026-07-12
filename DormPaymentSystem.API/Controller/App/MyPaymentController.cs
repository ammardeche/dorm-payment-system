using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.AuthController;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Enums;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller.App
{
    [ApiController]
    public class MyPaymentController : AppControllerBase
    {

        private readonly IPaymentService _paymentService;
        private readonly IReservationService _reservationService;


        public MyPaymentController(IPaymentService paymentService, IReservationService reservationService)
        {
            _paymentService = paymentService;
            _reservationService = reservationService;
        }


        [HttpGet("my-payments")]
        public async Task<IActionResult> GetMyPayments(
            [FromQuery] PaymentStatus? status,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var studentId = GetCurrentStudentId();

            // get the active reservation for the student

            var reservation = await _reservationService.GetActiveReservationByStudentIdAsync(studentId);

            if (reservation == null)
                return Ok(new Response<string>("No active reservation found."));

            var (payments, totalCount) = await _paymentService.GetAllPaymentsAsync(
                reservationId: reservation.Id,
                status: status,
                pageIndex: pageIndex,
                pageSize: pageSize

            ); // Assuming a maximum of 100 payments for simplicity

            var pagination = new Pagination(pageIndex, pageSize, totalCount);
            if (payments == null)
            {
                return NotFound(new Response<string>("No payments found."));
            }

            return Ok(new Response<IEnumerable<PaymentResponse>>(payments.Select(p => new PaymentResponse(p)).ToList()));
        }

        [HttpGet("pending")]

        public async Task<IActionResult> GetMyPendingPayments(
        )
        {

            var studentId = GetCurrentStudentId();

            // get the active reservation for the student 

            var reservation = await _reservationService.GetActiveReservationByStudentIdAsync(studentId);

            if (reservation == null)
                return Ok(new Response<string>("No active reservation found."));

            var (payments, _) = await _paymentService.GetAllPaymentsAsync(
                reservationId: reservation.Id,
                status: PaymentStatus.Pending,
                pageIndex: 1,
                pageSize: 100
            );

            return Ok(new Response<IEnumerable<PaymentResponse>>(payments.Select(p => new PaymentResponse(p)).ToList()));
        }

        // get summary of total amount 
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var studentId = GetCurrentStudentId();

            // get the active reservation for the student

            var reservation = await _reservationService.GetActiveReservationByStudentIdAsync(studentId);

            if (reservation == null)
                return Ok(new Response<string>("No active reservation found."));

            var (payments, _) = await _paymentService.GetAllPaymentsAsync(
                reservationId: reservation.Id,
                pageSize: 1000
            );
            // amount 
            var totalExpected = payments.Sum(p => p.Amount); // ALL generated invoices
            var TotalPaid = payments.Where(p => p.Status == PaymentStatus.Paid).Sum(p => p.Amount);
            var TotalPending = payments.Where(p => p.Status == PaymentStatus.Pending).Sum(p => p.Amount);
            var summary = new PaymentSummaryResponse
            {
                TotalPaid = TotalPaid,
                TotalPending = TotalPending,
                PendingCount = payments.Count(p => p.Status == PaymentStatus.Pending),
                OverdueCount = payments.Count(p => p.Status == PaymentStatus.Overdue),


                TotalExpected = totalExpected,
                RemainingBalance = totalExpected - TotalPaid
            };

            return Ok(new Response<PaymentSummaryResponse>(summary));
        }

    }
}