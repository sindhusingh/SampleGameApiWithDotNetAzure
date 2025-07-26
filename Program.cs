using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

using SampleGameApiWithDotNetAzure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
        limiterOptions.PermitLimit = 5; // max 5 requests
        limiterOptions.Window = TimeSpan.FromSeconds(10); // every 10 seconds
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2; // allow 2 requests to queue
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();


// ✅ Sample POST endpoint for Unity game score submission
app.MapPost("/submit-score", (ScoreSubmission submission) =>
{
    // For now, just echo back what was submitted.
    return Results.Ok(new
    {
        Message = $"Score received for {submission.PlayerId} with score {submission.Score}",
        ReceivedAt = DateTime.UtcNow
    });
})
.RequireRateLimiting("fixed")
.WithName("SubmitScore")
.WithOpenApi();

app.Run();
