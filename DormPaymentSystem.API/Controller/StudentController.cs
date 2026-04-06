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
    [Route("api/students")]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStudents([FromQuery] StudentQuery q)
        {
            var students = await _studentService.GetAllStudentsAsync(q.RoomId, q.IsActive, q.StudentNumber);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return Ok(new StudentResponse(student));
        }

        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentRequest req)
        {
            await _studentService.UpdateStudentAsync(
                id,
                req.FirstName,
                req.LastName,
                req.Email,
                req.PhoneNumber,
                req.RoomId
            );

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok("student updated successfully");
        }

        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return Ok("student removed successfully");
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateStudent(int id, DeactivateStudentRequest req)
        {
            await _studentService.DeactivateStudentAsync(id, req.DepartureDate);

            return Ok("student no longer active ");
        }

    }
}