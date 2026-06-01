using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.DTOs.Request
{
    public class CreateStudentReservationRequest
    {
        [Required] public int StudentId { get; set; }
        [Required] public int RoomId { get; set; }
        [Required] public DateTime CheckIn { get; set; }
        [Required][Range(1, 12)] public int Months { get; set; }
        [Required] public string ReceivedByUserId { get; set; } = null!;
    }
}