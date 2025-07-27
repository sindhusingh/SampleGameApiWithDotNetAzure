using System;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using SampleGameApiWithDotNetAzure.Data;


// Load .env file
DotNetEnv.Env.Load(); // 🔁 Must be first!

var builder = WebApplication.CreateBuilder(args);

// ✅ Read from configuration or hardcode for now
var cosmosConnectionString = builder.Configuration["CosmosDb:ConnectionString"];
var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
var containerName = builder.Configuration["CosmosDb:ContainerName"];

// Add services to the container.
builder.Services.AddControllers(); // Enables attribute-based API controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------- Rate Limiting Configuration -------------------
// .NET 8+ includes built-in support for rate limiting via middleware.
// This protects APIs from abuse, brute-force attacks, or misbehaving clients.
//
// We're currently using a **Fixed Window** policy:
// - Max 5 requests allowed every 10 seconds (PermitLimit & Window).
// - Up to 2 additional requests are queued and handled in order.
// - Clients exceeding this limit will receive HTTP 429 (Too Many Requests).
//
// This setup is ideal for basic rate limiting scenarios.
//
// ------------------- OPTIONAL ENHANCEMENTS (for future) -------------------
// ✅ Sliding Window Limiting:
//    - Smoother rate control by spreading limits over time.
//
// ✅ Token-Based Identity Limiting (e.g., per user/player):
//    - Instead of limiting per IP, extract Player ID or Auth token
//      from the request and limit each user individually.
//
// ✅ Concurrency Limiter:
//    - Controls how many concurrent requests can be handled at once.
//
// ✅ Custom Rejection Responses:
//    - Customize 429 response with JSON message: {"error": "Rate limit exceeded"}
//    - Provide "Retry-After" header to guide clients.
//
// ✅ Distributed Rate Limiting (e.g., with Redis):
//    - Useful for apps deployed on multiple instances or scaled out.
//
// ---------------------------------------------------------------

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5; // Max 5 requests per window
        limiterOptions.Window = TimeSpan.FromSeconds(10); // 10-second window
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2; // Queue 2 extra requests
    });
});

// ✅ Register CosmosClient
builder.Services.AddSingleton(s =>
{
    return new CosmosClient(cosmosConnectionString);
});

// ✅ Register your IPlayerRepository implementation
builder.Services.AddSingleton<IPlayerRepository>(s =>
{
    var client = s.GetRequiredService<CosmosClient>();
    return new CosmosPlayerRepository(client, databaseName, containerName);
});

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//Allow Swagger for all
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRateLimiter();       // Enables global rate limiting
//app.UseAuthorization();     // Ready for future JWT/Auth use
//app.UseMiddleware<PlayFabAuthMiddleware>(); //PlayFab Auth

app.MapControllers();       // Maps [ApiController] routes from controller files

// Fallback route for unmatched requests
app.MapFallback(() =>
{
    return Results.NotFound(new
    {
        error = "The requested endpoint does not exist.",
        hint = "Check your URL or refer to /swagger for available APIs."
    });
});


app.Run();
