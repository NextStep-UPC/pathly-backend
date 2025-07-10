using System;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pathly_backend.Analytics.Application;
using pathly_backend.Analytics.Application.Interfaces;
using pathly_backend.Shared.Common;
using pathly_backend.Shared.ErrorHandling;
using pathly_backend.IAM.Domain.Entities;
using pathly_backend.IAM.Domain.Enums;
using pathly_backend.IAM.Application;
using pathly_backend.IAM.Domain.Repositories;
using pathly_backend.IAM.Infrastructure.Persistence;
using pathly_backend.IAM.Infrastructure.Persistence.Repositories;
using pathly_backend.IAM.Infrastructure.Services;
using pathly_backend.Profile.Application;
using pathly_backend.Profile.Domain.Repositories;
using pathly_backend.Profile.Infrastructure.Persistence;
using pathly_backend.Profile.Infrastructure.Repositories;
using pathly_backend.SanctionsAndAppeals.Application.Interfaces;
using pathly_backend.SanctionsAndAppeals.Application.Services;
using pathly_backend.SanctionsAndAppeals.Domain.Repositories;
using pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence;
using pathly_backend.SanctionsAndAppeals.Infrastructure.Persistence.Repositories;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Application;
using pathly_backend.Sessions.Domain.Repositories;
using pathly_backend.Sessions.Infrastructure.Persistence;
using pathly_backend.Sessions.Infrastructure.Persistence.Repositories;
using pathly_backend.Sessions.Interfaces.SignalR;
using pathly_backend.VocationalTests.Application;
using pathly_backend.VocationalTests.Application.Interfaces;
using pathly_backend.VocationalTests.Domain.Repositories;
using pathly_backend.VocationalTests.Infrastructure.Persistence;
using pathly_backend.VocationalTests.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
var cfg     = builder.Configuration;

// ---------------------------------------------------------------------
// 1. DbContexts
// ---------------------------------------------------------------------
builder.Services.AddDbContext<IamDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));

builder.Services.AddDbContext<ProfileDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));

builder.Services.AddDbContext<SessionsDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));

builder.Services.AddDbContext<SanctionsDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));

builder.Services.AddDbContext<VocationalTestsDbContext>(opt =>
    opt.UseMySql(cfg.GetConnectionString("Default"),
        ServerVersion.AutoDetect(cfg.GetConnectionString("Default"))));

// ---------------------------------------------------------------------
// 2. Repositorios y UnitOfWork
// ---------------------------------------------------------------------
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IProfileRepository, EfProfileRepository>();

builder.Services.AddScoped<ISessionRepository, EfSessionRepository>();
builder.Services.AddScoped<IChatMessageRepository, EfChatMessageRepository>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IFeedbackRepository, EfFeedbackRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<INotificationRepository, EfNotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IReportRepository, EfReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddScoped<ITestRepository, EfTestRepository>();
builder.Services.AddScoped<IStudentTestRepository, EfStudentTestRepository>();
builder.Services.AddScoped<IVocationalTestsUnitOfWork, VocationalTestsUnitOfWork>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IStudentTestService, StudentTestService>();

builder.Services.AddScoped<ISanctionRepository, EfSanctionRepository>();
builder.Services.AddScoped<ISanctionService, SanctionService>();
builder.Services.AddScoped<IAppealRepository, EfAppealRepository>();
builder.Services.AddScoped<IAppealService, AppealService>();

builder.Services.AddScoped<IUnitOfWork, 
    pathly_backend.IAM.Infrastructure.Persistence.UnitOfWork>();
builder.Services.AddScoped<IProfileUnitOfWork, 
    pathly_backend.Profile.Infrastructure.Persistence.UnitOfWork>();
builder.Services.AddScoped<ISessionsUnitOfWork, 
    pathly_backend.Sessions.Infrastructure.Persistence.UnitOfWork>();

// ---------------------------------------------------------------------
// 3. Servicios de aplicación
// ---------------------------------------------------------------------
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProfileService>();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<SessionService>();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// ---------------------------------------------------------------------
// 4. SignalR
// ---------------------------------------------------------------------
builder.Services.AddSignalR();

// ---------------------------------------------------------------------
// 5. JWT
// ---------------------------------------------------------------------
var key = Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidIssuer              = cfg["Jwt:Issuer"],
            ValidateAudience         = true,
            ValidAudience            = cfg["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(key),
            ClockSkew                = TimeSpan.Zero
        };
        o.Events = new JwtBearerEvents
        {
            OnChallenge = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode  = 401;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync("{\"message\":\"Token inválido o ausente.\"}");
            }
        };
    });

// ---------------------------------------------------------------------
// 6. Swagger
// ---------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "PATHLY BACKEND", Version = "v1" });
    options.EnableAnnotations();

    var jwtScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme       = "bearer",
        BearerFormat = "JWT",
        Name         = "Authorization",
        In           = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type         = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description  = "Introduce tu token JWT. Ejemplo: **Bearer <token>**",
        Reference    = new()
        {
            Id   = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
    options.AddSecurityRequirement(new()
    {
        { jwtScheme, Array.Empty<string>() }
    });
});

// ---------------------------------------------------------------------
// 7. MVC + CORS
// ---------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", pb =>
        pb.SetIsOriginAllowed(origin => 
                new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

// ---------------------------------------------------------------------
// 8. Build app
// ---------------------------------------------------------------------
var app = builder.Build();

// ---------------------------------------------------------------------
// 9. Migraciones auto
// ---------------------------------------------------------------------
await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<IamDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ProfileDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<SessionsDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<SanctionsDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<VocationalTestsDbContext>().Database.MigrateAsync();
}

// ---------------------------------------------------------------------
// 10. Seed de Administrador
// ---------------------------------------------------------------------
await SeedAdminAsync(app);

// ---------------------------------------------------------------------
// 11. Pipeline HTTP
// ---------------------------------------------------------------------
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Front");

app.UseAuthentication();

// ---------------------------------------------------------------------
// 12. Middleware de bloqueo global por sanciones
// ---------------------------------------------------------------------
app.Use(async (HttpContext ctx, RequestDelegate next) =>
{
    if (ctx.User?.Identity?.IsAuthenticated ?? false)
    {
        var path = ctx.Request.Path.Value?.ToLowerInvariant() ?? "";
        var isAllowedWhenBanned =
            path.StartsWith("/api/sanctions/me") ||
            (path.StartsWith("/api/sanctions/") && path.Contains("/appeal")) ||
            path.StartsWith("/api/auth/logout");

        if (!isAllowedWhenBanned)
        {
            var userId = Guid.Parse(ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ban    = await ctx.RequestServices
                .GetRequiredService<ISanctionService>()
                .GetActiveByUserAsync(userId);
            if (ban != null && ban.IsActive)
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                await ctx.Response.WriteAsJsonAsync(new
                {
                    message = $"Estás sancionado hasta {ban.EndAtUtc?.ToString("u") ?? "permanente"} por: {ban.Reason}"
                });
                return;
            }
        }
    }

    await next(ctx);
});

app.UseAuthorization();

app.MapControllers();

app.MapHub<SessionChatHub>("/hubs/sessionChat");

app.Run();

// ---------------------------------------------------------------------
// 13. Función local: SeedAdminAsync
// ---------------------------------------------------------------------
async Task SeedAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var uow  = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    const string adminEmail    = "admin@pathly.com";
    const string adminPassword = "password!";

    if (!await repo.ExistsAsync(adminEmail))
    {
        var admin = User.Register(
            email:    adminEmail,
            password: adminPassword,
            first:    "Pathly",
            last:     "Admin");

        admin.GrantRole(UserRole.Admin);

        await repo.AddAsync(admin);
        await uow.SaveChangesAsync();

        Console.WriteLine($"Admin sembrado: {adminEmail} / {adminPassword}");
    }
}