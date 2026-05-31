using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.common
{
    public sealed record Pagination(int PageSize, int PageIndex, int TotalCount);
}