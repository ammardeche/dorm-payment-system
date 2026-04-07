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
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {


        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments([FromQuery] PaymentQuery q)
        {
            var payments = await _paymentService.GetAllPaymentsAsync(
            studentId: q.StudentId,
            guestId: q.GuestId,
            status: q.Status,
            startDate: q.StartDate,
            endDate: q.EndDate,
            receivedById: q.ReceivedById,
            dueToday: q.DueToday
            );
            return Ok(payments.Select(p => new PaymentResponse(p)));
        }

        // GET: api/payments/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            return Ok(new PaymentResponse(payment));
        }

        // GET: api/payments/receipt/{receiptNumber}
        [HttpGet("receipt/{receiptNumber}")]
        public async Task<IActionResult> GetPaymentByReceipt(string receiptNumber)
        {
            var payment = await _paymentService.GetPaymentByReceiptNumberAsync(receiptNumber);
            return Ok(new PaymentResponse(payment));
        }

        // POST: api/payments
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest dto)
        {
            var payment = await _paymentService.ProcessPaymentAsync(
                dto.StudentId,
                dto.Amount,
                dto.Month,
                dto.Year,
                dto.Method,
                dto.ReceivedByUserId
            );

            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, new PaymentResponse(payment));
        }

        // PUT: api/payments/{id}
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentRequest dto)
        {
            var updated = await _paymentService.UpdatePaymentAsync(
                id,
                dto.Amount,
                dto.Month,
                dto.Year,
                dto.Method,
                dto.Status,
                dto.Note
            );

            return Ok(new PaymentResponse(updated));
        }

        // DELETE: api/payments/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePayment(int id, [FromQuery] string deletedById)
        {
            var result = await _paymentService.DeletePaymentAsync(id, deletedById);
            return result ? NoContent() : BadRequest("Unable to delete payment");
        }

        // GET: api/payments/total?startDate=...&endDate=...
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalCollected([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var total = await _paymentService.GetTotalCollectedAsync(startDate, endDate);
            return Ok(total);
        }

        // GET: api/payments/exists/{id}
        [HttpGet("exists/{id:int}")]
        public async Task<IActionResult> PaymentExists(int id)
        {
            var exists = await _paymentService.PaymentExistsAsync(id);
            return Ok(exists);
        }
    }

}