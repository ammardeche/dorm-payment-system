using System.Text.Json.Serialization;
using DormPaymentSystem.API.Configurations;
using DormPaymentSystem.API.Middleware;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Seeders;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
   .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddOpenApi();
builder.Services.AddApplicationService();
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddDataConfiguration(builder.Configuration);
builder.Services.AddCorsConfiguration();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    await AdminSeeder.SeedAdmin(userManager);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
