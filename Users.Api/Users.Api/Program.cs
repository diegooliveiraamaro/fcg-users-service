using Microsoft.EntityFrameworkCore;
using Users.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================
// REGISTRO DE SERVIÇOS
// =======================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext ? SEMPRE antes do Build
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")
    )
);

// =======================
// BUILD
// =======================
var app = builder.Build();

// =======================
// MIDDLEWARE
// =======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
