var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

// Configuration
var displayServiceUrl = builder.Configuration["DisplayServiceUrl"] ?? "http://localhost:5010";

// Ensure backupLog directory exists for request logging
var backupLogDirectory = Path.Combine(AppContext.BaseDirectory, "backupLog");
Directory.CreateDirectory(backupLogDirectory);

// API endpoint to receive name and forward it to DisplayService
app.MapPost("/api/submitname", async (NameRequest request, IHttpClientFactory httpClientFactory) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new { error = "Name is required" });
    }

    try
    {
        // Write a log entry per submission
        var logFilePath = Path.Combine(backupLogDirectory, $"{DateTime.UtcNow:yyyyMMdd}.log");
        var logLine = $"{DateTime.UtcNow:o} | name=\"{request.Name}\"";
        await File.AppendAllTextAsync(logFilePath, logLine + Environment.NewLine);

        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.PostAsJsonAsync($"{displayServiceUrl}/api/displayname", new { Name = request.Name });
        
        if (response.IsSuccessStatusCode)
        {
            return Results.Ok(new { message = "Name sent successfully", name = request.Name });
        }
        
        return Results.BadRequest(new { error = "Failed to send name to display service" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error communicating with display service: {ex.Message}");
    }
})
.WithName("SubmitName")
.WithOpenApi();

app.Run();

record NameRequest(string Name);