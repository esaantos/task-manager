using TaskManager.API.ExceptionHandler;
using TaskManager.API.Exceptions;
using TaskManager.Application;
using TaskManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("gerente"));
});

builder.Services.AddControllers();
//builder.Services.AddExceptionHandler<APIExceptionHandler>();
builder.Services.AddProblemDetails();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>(new APIExceptionHandler());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
