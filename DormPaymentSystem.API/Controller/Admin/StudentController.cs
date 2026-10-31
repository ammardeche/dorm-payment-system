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
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static DormPaymentSystem.Core.Exceptions.AppException;

namespace DormPaymentSystem.API.Controller
{
    public class StudentController : AdminControllerBase
    {

        private readonly IStudentService _studentService;
        private readonly IAuthService _authService;

        public StudentController(IStudentService studentService, IAuthService authService)

        {
            _studentService = studentService;
            _authService = authService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStudents([FromQuery] StudentQuery q)
        {
            var (items, totalCount) = await _studentService.GetAllStudentsAsync(
                q.RoomId, q.IsActive, q.StudentNumber, q.PageIndex, q.PageSize);

            var pagination = new Pagination(q.PageSize, q.PageIndex, totalCount);

            return Ok(new Response<IEnumerable<StudentResponse>>(
                items.Select(s => new StudentResponse(s)),
                pagination));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return Ok(new Response<StudentResponse>(new StudentResponse(student)));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Fill all required fields.")));

            var student = await _studentService.CreateStudentWithAccountAsync(
    req.FirstName, req.LastName, req.Email,
    req.StudentNumber, req.PhoneNumber, req.Password);

return Ok(new Response<StudentResponse>(new StudentResponse(student)));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response(new AppValidationException("Invalid request data.")));

            await _studentService.UpdateStudentAsync(
                id,
                req.FirstName,
                req.LastName,
                req.Email,
                req.PhoneNumber
               );

            return Ok(new Response<string>("Student updated successfully."));
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateStudent(int id, [FromBody] DeactivateStudentRequest req)
        {
            await _studentService.DeactivateStudentAsync(id, req.DepartureNote);
            return Ok(new Response<string>("Student deactivated successfully."));
        }

        [HttpPost("reactivate/{id}")]
public async Task<IActionResult> ReactivateStudent(int id)
{
    await _studentService.ActivateStudentAsync(id);
    return Ok(new Response<string>("Student reactivated successfully."));
}

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return Ok(new Response<string>("Student removed successfully."));
        }

        [HttpGet("stats")]

        public async Task<IActionResult> GetStudentStats()
        {
            var stats = await _studentService.GetStudentStats();
            return Ok(new Response<StudentStatsResponse>(stats));
        }
    }

}
