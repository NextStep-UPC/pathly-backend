using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MediatR;
using pathly_backend.IAM.Infrastructure.Persistence.Context;
using pathly_backend.IAM.Application.Internal.Interfaces;
using pathly_backend.IAM.Application.Internal.Services;
using pathly_backend.IAM.Infrastructure.Security;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
                               
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "pathly-backend",
        Version = "1.0.0"
    });

    options.EnableAnnotations();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Obtiene la de conexión de appsettings.json para el MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra el DbContext del bounded context IAM (sign-up y sign-in)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Ejemplo -> Crean un bounded context llamado "Ejemplo" usando:
//
// public class EjemploDbContext : DbContext
// {
//     public DbSet<Ejemplo> Ejemplo { get; set; }
//     public EjemploDbContext(DbContextOptions<EjemploDbContext> options)
//         : base(options) { }
// }
//
// Y lo registran aquí exactamente igual:
//builder.Services.AddDbContext<EjemploDbContext>(options =>
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
//
// Esto permitirá que tanto IAM como Ejemplo usen la misma base de datos
// (se crearán sus tablas en paralelo dentro de "pathly_backend")
//
// Pueden fijarse como funciona mi "ApplicationDbContext.cs" en:
// Infrastructure -> Persistence -> Context -> ApplicationDbContext.cs


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<pathly_backend.Psychologist.Domain.Services.SectionService>();
builder.Services.AddScoped<pathly_backend.Psychologist.Domain.Services.UpdateSectionHandler>();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>();

builder.Services.AddSingleton(jwtOptions);
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
    };
});

builder.Services.AddMediatR(cfg => { }, 
    typeof(pathly_backend.Psychologist.Domain.Model.Commands.CreateSectionCommand).Assembly,
    typeof(pathly_backend.Psychologist.Domain.Model.Commands.UpdateSectionCommand).Assembly,
    typeof(pathly_backend.Psychologist.Domain.Services.SectionService).Assembly
);

// preserve referencias puede ser eliminado sin problemas pero se ve mas bonito asi depende de ustedes
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

// Aplica migraciones automáticamente al iniciar el proyecto
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // Crea la base de datos y tablas si no existen
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();   

app.Run();
