using System.Text.Json.Serialization;
using DormPaymentSystem.API.Configurations;
using DormPaymentSystem.API.Middleware;
using DormPaymentSystem.Core.Entities;
using DormPaymentSystem.Data.Seeders;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Services
builder.Services.AddOpenApi();
builder.Services.AddApplicationService();
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddDataConfiguration(builder.Configuration);

// ✅ CORS (INLINE — no mystery method)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // Angular app
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // only if using cookies/auth
    });
});

var app = builder.Build();

// Seed admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    await AdminSeeder.SeedAdmin(userManager);
}

// Dev tools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware pipeline (ORDER MATTERS ⚠️)
app.UseMiddleware<ExceptionHandlerMiddleware>();

// ❗ IMPORTANT: Keep this, but frontend MUST use HTTPS
// app.UseHttpsRedirection();

// ✅ CORS MUST be before auth
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();