using SampleGameApiWithDotNetAzure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
.WithName("SubmitScore")
.WithOpenApi();

app.Run();
