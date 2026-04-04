using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.DTOs.Request;
using DormPaymentSystem.API.DTOs.Response;
using DormPaymentSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DormPaymentSystem.API.Controller
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students.Select(p => new StudentResponse(p)));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest req)
        {
            var student = await _studentService.CreateStudentAsync(
                req.FirstName,
                req.LastName,
                req.Email,
                req.StudentNumber,
                req.PhoneNumber,
                req.EnrollmentDay,
                req.RoomId
            );
            if (!ModelState.IsValid)
            {
                return BadRequest("make sure to fill the required fields");
            }
            return Ok(new StudentResponse(student));
        }
    }
}