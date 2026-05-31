using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.API.Queries
{



    public class StudentQuery
    {
        public int? RoomId { get; set; }
        public bool? IsActive { get; set; }
        public string? StudentNumber { get; set; }

        // pagination
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }




}