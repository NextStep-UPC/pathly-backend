using Admin.Domain.Model.Queries;
using Admin.Infrastructure.Persistence;
using IAM.Application.Services;
using IAM.Domain.Repositories;
using IAM.Infrastructure;
using IAM.Infrastructure.Persistence;
using IAM.Infrastructure.Persistence.Repositories;
using IAM.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

builder.Services.AddDbContext<IamDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));

builder.Services.AddScoped<GetUserStatsQuery>();
builder.Services.AddScoped<GetSessionsQuery>();
builder.Services.AddScoped<GetFeedbackQuery>();
builder.Services.AddDbContext<AdminDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));


builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtTokenGenerator>();

var key = Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = cfg["Jwt:Issuer"],
            ValidAudience = cfg["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        o.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\":\"Token inválido o ausente.\"}");
            }
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "PATHY BACKEND", Version = "v1" });

    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "Introduce tu token JWT. Ejemplo: Bearer {token}",

        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddControllers();
builder.Services.AddCors(p => p.AddPolicy("Front",
    pb => pb.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IamDbContext>();
    await db.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Front");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();