using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Enums;

namespace DormPaymentSystem.API.Queries
{
    public class GuestQuery
    {
        public string? DocumentNumber { get; set; }
        public DocumentType DocumentType { get; set; }
        public string? FullName { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}