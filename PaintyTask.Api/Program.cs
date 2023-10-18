using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using PaintyTask.Api.Helpers;
using PaintyTask.Api.Helpers.Interfaces;
using PaintyTask.Application.Mapping;
using PaintyTask.Application.Services;
using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Application.Validators;
using PaintyTask.Application.Validators.Interfaces;
using PaintyTask.Domain.Models.Configurations;
using PaintyTask.Infrastructure.Context;
using PaintyTask.Infrastructure.Repositories;
using PaintyTask.Infrastructure.Repositories.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "PaintyTask API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },

            Array.Empty<string>()
        }
    });

    option.EnableAnnotations();
});

var mapper = new Mapper(
    new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<AutoMapperProfile>();
    }));
builder.Services.AddSingleton(mapper);

var jwtConfig = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtConfig>()!;
builder.Services.AddSingleton(jwtConfig);

var imgCongig = builder.Configuration
    .GetSection("ImageSettings")
    .Get<ImageConfig>()!;
builder.Services.AddSingleton(imgCongig);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IValidate, Validate>();
builder.Services.AddScoped<IAuthorizationHelper, AuthorizationHelper>();
builder.Services.AddScoped<ISaveImgService, SaveImgService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();