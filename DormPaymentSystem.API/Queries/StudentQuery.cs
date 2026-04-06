using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.Queries
{



    public class StudentQuery
    {
        public int? RoomId { get; set; } = null;
        public bool? IsActive { get; set; } = null;
        public string? StudentNumber { get; set; } = null;
    }




}