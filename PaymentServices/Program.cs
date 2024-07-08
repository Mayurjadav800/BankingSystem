using AutoMapper;
using BankingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PaymentMicroServices.Data;
using PaymentMicroServices.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetService<IConfiguration>();
builder.Services.AddDbContext<PaymentDbContext>(Options => Options.UseSqlServer(builder
    .Configuration.GetConnectionString("PaymentConnection")).EnableSensitiveDataLogging());
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ICompundRepository, CompundRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
