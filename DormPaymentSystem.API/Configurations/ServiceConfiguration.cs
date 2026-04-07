using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Interfaces;
using DormPaymentSystem.Core.Services;
using DormPaymentSystem.Data.Interfaces;
using DormPaymentSystem.Data.Repositories;

namespace DormPaymentSystem.API.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection _service)
        {


            // register services and repositories 

            // floor 
            _service.AddScoped<IFloorService, FloorService>();
            _service.AddScoped<IFloorRepository, FloorRepository>();
            // room 
            _service.AddScoped<IRoomRepository, RoomRepository>();
            _service.AddScoped<IRoomService, RoomService>();
            // payment
            _service.AddScoped<IPaymentRepository, PaymentRepository>();
            _service.AddScoped<IPaymentService, PaymentService>();
            // student 
            _service.AddScoped<IStudentService, StudentService>();
            _service.AddScoped<IStudentRepository, StudentRepository>();
            // user


            // provide services 

            return _service;
        }
    }
}