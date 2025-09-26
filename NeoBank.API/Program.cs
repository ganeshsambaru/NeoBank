using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Data;
using NeoBank.Api.Repositories.Interfaces;
using NeoBank.Api.Repositories.Implementations;
using NeoBank.Api.Services.Interfaces;
using NeoBank.Api.Services.Implementations;
using Serilog;
using NeoBankApi.Repositories;
using NeoBankApi.Services;

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

// EF Core DbContext (connection string in appsettings.json)
builder.Services.AddDbContext<NeoBankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Controllers & Swagger
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // prevents JSON self-referencing loop errors
        o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS: allow Angular + React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// ---------------------
// Configure HTTP pipeline
// ---------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global exception middleware — put early so it catches errors
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
