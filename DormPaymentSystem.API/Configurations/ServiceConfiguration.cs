using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.API.Infrastructure;
using DormPaymentSystem.Core.Entities;
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
            // notification
            _service.AddScoped<INotificationService, NotificationService>();
            _service.AddScoped<INotificationRepository, NotificationRepository>();
            // guest 
            _service.AddScoped<IGuestService, GuestService>();
            _service.AddScoped<IGuestRepository, GuestRepository>();
            // invitation
            _service.AddScoped<IInvitationService, InvitationService>();
            _service.AddScoped<IInvitationRepository, InvitationRepository>();
            // auth 
            _service.AddScoped<IAuthService, AuthService>();
            // user 
            _service.AddScoped<IUserService, UserService>();
            // token
            _service.AddScoped<ITokenService, TokenService>();


            return _service;
        }
    }
}