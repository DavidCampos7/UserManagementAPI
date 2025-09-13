using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de controladores
builder.Services.AddControllers();

// Agregar autenticación JWT (si usas [Authorize])
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "tu_issuer",
            ValidAudience = "tu_audience",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes("EsteEsUnSecretoMuySeguroYConMásDe32Caracteres"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware global de manejo de errores
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.Use(async (context, next) =>
{
    Console.WriteLine("🔍 Middleware de diagnóstico:");
    Console.WriteLine($"  IsAuthenticated: {context.User.Identity?.IsAuthenticated}");
    Console.WriteLine($"  Claims: {string.Join(", ", context.User.Claims.Select(c => $"{c.Type}={c.Value}"))}");

    await next();
});

app.UseAuthorization();

// Middleware de validación de token para rutas seguras
app.UseMiddleware<TokenValidationMiddleware>();

// Middleware de auditoría personalizado
app.UseMiddleware<AuditMiddleware>();


// Mapea las rutas de los controladores
app.MapControllers();

// Ruta raíz opcional
app.MapGet("/", () => "Hello World!");

app.Run();
