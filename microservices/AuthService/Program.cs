using AuthService.Repository;
using AuthService.Service;
using AuthService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 🧹 Clear Logging for clean output ---
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
builder.Logging.AddFilter("System", LogLevel.Warning);
builder.Logging.AddConsole();

// --- 🔐 Configure JWT ---
var jwtKey = builder.Configuration["Jwt:Key"] ?? "THIS_IS_A_SECURE_32_CHARACTER_KEY_!!!";
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddSingleton(new JwtService(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// --- 🗄️ Database ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=127.0.0.1,1433;Database=AuthServiceDB;User Id=sa;Password=Your_Password123;TrustServerCertificate=True;";

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 🏗️ Dependency Injection ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService.Service.AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 📑 Swagger ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Redirect root to swagger
app.MapGet("/", context => {
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

Console.WriteLine("Database connected successfully");
Console.WriteLine("🚀 Auth Service is running at: http://localhost:5001");
Console.WriteLine("👉 Swagger link: http://localhost:5001/swagger");

app.Run();
