using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Repositories
{
    public class GuestRepository : IGuestRepository
    {

        private readonly DormPaymentDbContext _context;
        public GuestRepository(DormPaymentDbContext context)
        {
            _context = context;
        }




    }
}


