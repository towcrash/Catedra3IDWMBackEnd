using System.Security.Claims;
using System.Text;
using catedra3.src.data;
using catedra3.src.interfaces;
using catedra3.src.models;
using catedra3.src.repository;
using catedra3.src.service;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var cloudName = Environment.GetEnvironmentVariable("CloudinaryName");
var apiKey = Environment.GetEnvironmentVariable("ApiKey");
var apiSecret = Environment.GetEnvironmentVariable("ApiSecret");


if (cloudName == null || apiKey == null || apiSecret == null)
{
    throw new Exception("Cloudinary settings not found in environment variables.");
}

var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
var cloudinary = new Cloudinary(cloudinaryAccount);

builder.Services.AddSingleton(cloudinary);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => 
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = 
        options.DefaultChallengeScheme = 
        options.DefaultForbidScheme =
        options.DefaultScheme = 
        options.DefaultSignInScheme = 
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(option => {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "")),
            RoleClaimType = ClaimTypes.Role
        };
    });
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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
            new string[]{}
        }
    });
});

string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source=app.db";
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:4200") 
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); 
        });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDBContext>();
    await context.Database.MigrateAsync();
    
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


