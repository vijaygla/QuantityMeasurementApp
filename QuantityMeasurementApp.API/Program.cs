using QuantityMeasurementApp.Service;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.API.Middleware;
using NLog;
using NLog.Web;

/// <summary>
/// Entry point for the Quantity Measurement Web API.
/// Why: To expose the measurement business logic to external clients (web, mobile, or third-party services) over HTTP.
/// How: Uses ASP.NET Core Minimal API and Controllers to provide RESTful endpoints, with Swagger for documentation.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// --- [LOGGING CONFIGURATION] ---
// Why: To track application behavior, capture errors, and facilitate debugging in production.
// How: Using NLog to write logs to both file and console as defined in Logging/NLog.config.
var logger = LogManager.Setup().LoadConfigurationFromFile("Logging/NLog.config").GetCurrentClassLogger();
builder.Logging.ClearProviders();
builder.Host.UseNLog();

try 
{
    // --- [SERVICES REGISTRATION] ---
    // Why: To enable dependency injection (DI) so controllers can focus on coordination without knowing implementation details.
    // How: Scoped lifetime ensures a new instance is created for each HTTP request, matching database connection patterns.
    builder.Services.AddControllers();

    // Swagger/OpenAPI setup for API discovery and testing.
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // --- [BUSINESS LOGIC DI] ---
    // Why: To ensure the API uses the same core logic and persistence layer as the console application.
    builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
    builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

    var app = builder.Build();

    // --- [DATABASE INITIALIZATION] ---
    // Why: Real-time apps must ensure the underlying storage is ready before accepting requests.
    // How: Calls the shared DatabaseInitializer to check/create SQL DB and tables automatically.
    QuantityMeasurementApp.Utilities.DatabaseInitializer.Initialize();

    // --- [MIDDLEWARE PIPELINE] ---
    // Why: To process requests in a consistent manner (security, logging, error handling).

    // Global Exception Handling Middleware
    // Why: To catch unhandled errors and return consistent JSON responses instead of exposing stack traces.
    app.UseMiddleware<ExceptionMiddleware>();

    // Enable Swagger in Development or Production based on needs.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Redirect HTTP to HTTPS for security.
    app.UseHttpsRedirection();

    // Mapping controllers to routes.
    app.MapControllers();

    // Default Landing Route
    // Why: To provide a quick sanity check if the API is running correctly.
    app.MapGet("/", () => "Quantity Measurement API is online and healthy.");

    logger.Info("Quantity Measurement API successfully started.");
    app.Run();
}
catch (Exception exception)
{
    // Capture any startup errors that prevent the app from booting.
    logger.Error(exception, "Stopped program because of exception during startup.");
    throw;
}
finally
{
    // Ensure the logger is properly closed when the app shuts down.
    LogManager.Shutdown();
}
