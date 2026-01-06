using Microsoft.EntityFrameworkCore;
using Users.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================
// REGISTRO DE SERVIÇOS
// =======================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
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

// ?? Swagger habilitado EM TODOS os ambientes (necessário para ECS)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
    c.RoutePrefix = "swagger"; // mantém /swagger
});

// ? Removido HTTPS redirection (ALB já termina SSL se existir)
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// ?? ESSENCIAL para Docker / ECS
app.Urls.Add("http://0.0.0.0:80");

app.Run();
