using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DormPaymentSystem.API.Configurations
{
    public static class DataConfiguration
    {
        public static IServiceCollection AddDataConfiguration(this IServiceCollection _services, IConfiguration _configuration)
        {

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // _services.AddDbContext<Nullable>(opt => opt.UseSqlServer(connectionString));

            return _services;
        }
    }
}