using Microsoft.EntityFrameworkCore;
using ab_project.Data;
using ab_project.Models;
using ab_project.Repositories;
using ab_project.Services;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHealthChecks();

// Function to get secret
string GetSecret()
{
    string secretName = "ab_project/dbcredentials_dev";
    string region = "us-west-2";

    IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

    GetSecretValueRequest request = new GetSecretValueRequest
    {
        SecretId = secretName,
        VersionStage = "AWSCURRENT",
    };

    try
    {
        var response = client.GetSecretValueAsync(request).Result;
        return response.SecretString;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error retrieving secret: {e.Message}");
        throw;
    }
}

// Retrieve the secret
var secretJson = GetSecret();

// Parse the JSON string to get the database credentials
var dbCredentials = JsonSerializer.Deserialize<Dictionary<string, string>>(secretJson);

// Construct the connection string
var connectionString = $"server={dbCredentials["host"]};" +
                       $"port={dbCredentials["port"]};" +
                       $"database={dbCredentials["dbname"]};" +
                       $"user={dbCredentials["username"]};" +
                       $"password={dbCredentials["password"]}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<ITrainingService, TrainingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Add health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = new[] { new { name = "application", status = "Healthy" } }
            });
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();