using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.Data.Data
{
    public class DormPaymentDbContext : IdentityDbContext
    {


        public DormPaymentDbContext(DbContextOptions<DormPaymentDbContext> options) : base(options)
        {

        }
    }
}