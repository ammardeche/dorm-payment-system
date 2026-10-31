using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Entities
{
    public class RoomStatsResponse
    {
     public int Total { get; set; }
    public int Available { get; set; }
    public int Full { get; set; }
    public int FreeBeds { get; set; }
    }
}