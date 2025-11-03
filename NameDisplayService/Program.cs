var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

// API endpoint to receive name from InputService
app.MapPost("/api/displayname", (NameRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new { error = "Name is required" });
    }

    NameStorage.CurrentName = request.Name;
    return Results.Ok(new { message = "Name received successfully", name = request.Name });
})
.WithName("DisplayName")
.WithOpenApi();

// API endpoint to get the current name
app.MapGet("/api/getname", () =>
{
    if (string.IsNullOrEmpty(NameStorage.CurrentName))
    {
        return Results.Ok(new { name = "" });
    }
    return Results.Ok(new { name = NameStorage.CurrentName });
})
.WithName("GetName")
.WithOpenApi();

app.Run();

record NameRequest(string Name);

// In-memory storage for the name (in a real scenario, you'd use a database or cache)
// Using a static class to persist the name across requests
static class NameStorage
{
    public static string? CurrentName { get; set; }
}
