using QuantityMeasurementApp.Service;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.API.Middleware;
using QuantityMeasurementApp.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NLog;
using NLog.Web;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Initialize DB config
DatabaseConfig.Initialize(builder.Configuration);

// Setup NLog
var logger = LogManager.Setup()
    .LoadConfigurationFromFile("Logging/NLog.config")
    .GetCurrentClassLogger();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

try
{
    // -------------------- Controllers + Swagger --------------------
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // -------------------- Repository + Service --------------------
    builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
    builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

    // -------------------- Auth (JWT) --------------------
    var jwtKey = builder.Configuration["Jwt:Key"] ?? "DEFAULT_SECRET_KEY";

    builder.Services.AddSingleton(new JwtService(jwtKey));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    var key = Encoding.UTF8.GetBytes(jwtKey);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
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

    // -------------------- Redis --------------------
    var redisConnection = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";

    builder.Services.AddSingleton<IConnectionMultiplexer>(
        ConnectionMultiplexer.Connect(redisConnection)
    );

    builder.Services.AddScoped<RedisCacheService>();

    // -------------------- RabbitMQ --------------------
    // Config values from appsettings.json
    var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
    var rabbitQueue = builder.Configuration["RabbitMQ:QueueName"] ?? "QuantityQueue";

    // Register Producer (send messages)
    builder.Services.AddSingleton(new RabbitMQProducer(rabbitHost, rabbitQueue));

    // Register Consumer (background processing)
    builder.Services.AddHostedService<RabbitMQConsumer>();

    // -------------------------------------------------

    var app = builder.Build();

    // Initialize DB
    DatabaseInitializer.Initialize();

    // Global exception middleware
    app.UseMiddleware<ExceptionMiddleware>();

    // Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Auth middleware
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Health check
    app.MapGet("/", () => "Quantity Measurement API is online and healthy.");

    logger.Info("API started successfully.");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Startup failed.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
