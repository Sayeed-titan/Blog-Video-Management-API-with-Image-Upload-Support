using MasterDetails.API.Data;
using MasterDetails.API.Interfaces;
using MasterDetails.API.Mapping;
using MasterDetails.API.Repositories;
using MasterDetails.API.Services;
using MasterDetails.API.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["jwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["jwtSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["jwtSettings:Key"]!)),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));




builder.Services.AddControllers()
     .AddNewtonsoftJson(option =>
     {
         option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
         option.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
     });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:5246", "http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyOrigin()
                                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Also register examples


builder.Services.AddDbContext<BlogDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("appCon")));

builder.Services.AddAutoMapper(typeof(BlogMappingProfile));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
