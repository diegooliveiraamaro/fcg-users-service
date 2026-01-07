using Microsoft.EntityFrameworkCore;
using Users.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// ✅ Health endpoint para ALB
app.MapGet("/health", () => Results.Ok("Healthy"));

// ESSENCIAL para Docker / ECS
app.Urls.Add("http://0.0.0.0:80");

app.Run();
