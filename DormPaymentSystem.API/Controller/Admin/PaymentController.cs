using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DormPaymentSystem.API.Controller.Admin;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.API.Queries;
using DormPaymentSystem.Core.common;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]
    public class PaymentController : AdminControllerBase
    {

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPayments([FromQuery] PaymentQuery q)
        {
            var (items, totalCount) = await _paymentService.GetAllPaymentsAsync(
                q.ReservationId, q.Status, q.StartDate,
                q.EndDate, q.ReceivedById, q.DueToday,
                q.PageIndex, q.PageSize);

            var pagination = new Pagination(q.PageSize, q.PageIndex, totalCount);

            return Ok(new Response<IEnumerable<PaymentResponse>>(
                items.Select(p => new PaymentResponse(p)),
                pagination));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            return Ok(new Response<PaymentResponse>(new PaymentResponse(payment)));
        }

        [HttpGet("receipt/{receiptNumber}")]
        public async Task<IActionResult> GetByReceipt(string receiptNumber)
        {
            var payment = await _paymentService.GetPaymentByReceiptNumberAsync(receiptNumber);
            return Ok(new Response<PaymentResponse>(new PaymentResponse(payment)));
        }

        // mark a pending payment as paid
        [HttpPost("mark-paid/{id}")]
        public async Task<IActionResult> MarkAsPaid(int id, [FromBody] MarkAsPaidRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var payment = await _paymentService.MarkAsPaidAsync(id, req.Method, userId);
            return Ok(new Response<PaymentResponse>(new PaymentResponse(payment)));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentRequest req)
        {
            var payment = await _paymentService.UpdatePaymentAsync(
                id, req.Amount, req.Method, req.Status, req.Note);
            return Ok(new Response<PaymentResponse>(new PaymentResponse(payment)));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _paymentService.DeletePaymentAsync(id, userId);
            return Ok(new Response<string>("Payment deleted successfully."));
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var total = await _paymentService.GetTotalCollectedAsync(startDate, endDate);
            return Ok(new Response<decimal>(total));
        }


    }



}



