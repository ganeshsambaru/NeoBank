using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Data;
using NeoBank.Api.Repositories.Interfaces;
using NeoBank.Api.Repositories.Implementations;
using NeoBank.Api.Services.Interfaces;
using NeoBank.Api.Services.Implementations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ---------------------
// Serilog Logging
// ---------------------
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

// ---------------------
// Add services to the container
// ---------------------

// EF Core DbContext (change connection string in appsettings.json)
builder.Services.AddDbContext<NeoBankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
// (later add IAccountRepository, etc.)

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
// (later add IAccountService, etc.)

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------
// CORS configuration
// ---------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200") // Angular dev URL
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// ---------------------
// Configure HTTP pipeline
// ---------------------

// Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom global exception middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Apply CORS before Authorization
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
