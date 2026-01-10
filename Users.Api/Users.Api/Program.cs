using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Users.Api.Application.Services;
using Users.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthService>();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();


//if (Debugger.IsAttached)
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
//        c.RoutePrefix = "swagger";
//    });
//}
//else
//{
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/users/swagger/v1/swagger.json", "Users API v1");
//    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
//    c.RoutePrefix = "swagger";
//});
//}


app.UseSwagger(c =>
{
    c.RouteTemplate = "users/swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/users/swagger/v1/swagger.json", "Users API v1");
    c.RoutePrefix = "users/swagger";
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// ✅ ENDPOINT ÚNICO PARA O ALB
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Urls.Add("http://0.0.0.0:80");

app.Run();


//using Microsoft.EntityFrameworkCore;
//using Users.Api.Infrastructure.Data;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<UsersDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
//);

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
//    c.RoutePrefix = "swagger";
//});

//app.UseRouting();
//app.UseAuthorization();

//app.MapControllers();

//// ✅ Health endpoint para ALB
//app.MapGet("/health", () => Results.Ok("Healthy"));

//// ESSENCIAL para Docker / ECS
//app.Urls.Add("http://0.0.0.0:80");

//app.Run();
