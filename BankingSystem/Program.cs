using BankingSystem.Data;
using BankingSystem.Model;
using BankingSystem.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//IMapper mapper = MappingConfiguration.RegisterMaps().CreateMapper();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
// Add services to the container.

builder.Services.AddControllers(); builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Practice.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
    }
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetService<IConfiguration>();

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));
builder.Services.AddSwaggerGen(); builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

builder.Services.AddAutoMapper(typeof(Program));
// Register the repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDepositeRepository, DepositeRepository>();
builder.Services.AddScoped<IWithdrawRepository, WithdraRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddTransient<IEmailRepository,EmailRepository>();

builder.Services.AddScoped<IOtpRepository, OtpRepository>();

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

// Configure Quartz
builder.Services.AddQuartz(q =>
{
    // Use a Scoped container to create jobs. 
    // base Quartz scheduler, job and trigger configuration
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Register the job, loading the schedule from configuration
    var jobKey = new JobKey("birthdayEmailJob", "group1");
    q.AddJob<BirthDayEmailJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("birthdayEmailTrigger", "group1")
        //This Cron interval can be described as "run every minute" (when second is zero)
        .WithCronSchedule("0 05 18 * * ?"));
});

// Add the Quartz hosted service
// when shutting down we want jobs to complete......
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();





