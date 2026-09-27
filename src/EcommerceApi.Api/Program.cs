using EcommerceApi.Application;
using EcommerceApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers
builder.Services.AddControllers();

// Register Application services and Infrastructure (EF Core DbContext, repositories)
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Register Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Enable Swagger Middleware in Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Serves the interactive Swagger web page
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
