using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Text;
using Users.Api.Application.Services;
using Users.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// 🔐 CONFIGURAÇÃO JWT
var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});


// 📘 SWAGGER + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Users API",
        Version = "v1",
        Description = "Microsserviço responsável por autenticação e usuários"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Ex: 'Bearer SEU_TOKEN_AQUI'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<AuthService>();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();


// 🔹 ESSENCIAL PARA INGRESS /users
app.UsePathBase("/users");


// 📘 Swagger (sempre ativo no cluster)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/users/swagger/v1/swagger.json", "Users API v1");
    c.RoutePrefix = "swagger";
});


app.UseRouting();

app.UseAuthentication(); // 🔐 IMPORTANTE
app.UseAuthorization();

app.MapControllers();

// ✅ Health check para ALB
app.MapGet("/health", () => Results.Ok("Healthy"));

// Docker / Kubernetes
app.Urls.Add("http://0.0.0.0:80");


// 🔄 Migration automática
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    db.Database.Migrate();
}

app.Run();

//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using Users.Api.Application.Services;
//using Users.Api.Infrastructure.Data;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<AuthService>();

//builder.Services.AddDbContext<UsersDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
//);

//var app = builder.Build();


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

//    app.UseSwagger(c =>
//    {
//        c.RouteTemplate = "users/swagger/{documentName}/swagger.json";
//    });

//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/users/swagger/v1/swagger.json", "Users API v1");
//        c.RoutePrefix = "users/swagger";
//    });
//}


//app.UseRouting();
//app.UseAuthorization();


//app.MapControllers();

//// ✅ ENDPOINT ÚNICO PARA O ALB
//app.MapGet("/health", () => Results.Ok("Healthy"));

//app.Urls.Add("http://0.0.0.0:80");

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
//    db.Database.Migrate();
//}

//app.Run();
