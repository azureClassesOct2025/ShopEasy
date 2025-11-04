var builder = WebApplication.CreateBuilder(args);

// Services
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

// Middleware
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

// Config: where to read logs from
// Default to NameInputService/bin/Debug/net8.0/backupLog relative to solution root
var defaultLogDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "NameInputService", "bin", "Debug", "net8.0", "backupLog"));
var logDirectory = builder.Configuration["LogDirectory"] ?? defaultLogDir;
Directory.CreateDirectory(logDirectory);

// List logs (filenames)
app.MapGet("/api/logs", () =>
{
	var files = Directory.GetFiles(logDirectory, "*.log")
		.Select(Path.GetFileName)
		.OrderByDescending(name => name)
		.ToArray();
	return Results.Ok(files);
}).WithName("ListLogs").WithOpenApi();

// Get log content by filename
app.MapGet("/api/logs/{file}", (string file) =>
{
	var safeName = Path.GetFileName(file);
	var path = Path.Combine(logDirectory, safeName);
	if (!System.IO.File.Exists(path))
	{
		return Results.NotFound();
	}
	var text = System.IO.File.ReadAllText(path);
	return Results.Text(text, "text/plain");
}).WithName("GetLog").WithOpenApi();

app.Run();
