using Library.DemoService;
using Library.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((cxt, cfg) => cfg.ReadFrom.Configuration(cxt.Configuration));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSdkControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IItemService, ItemService>();

builder.Services.AddSwaggerGen();
builder.Services.AddMiddleware();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.UseMiddleware();
app.Run();
