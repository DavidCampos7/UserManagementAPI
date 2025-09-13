namespace UserApi.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            var request = context.Request;

            _logger.LogInformation("Solicitud entrante: {method} {path} a las {time}",
                request.Method, request.Path, requestTime);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context); // Ejecuta el siguiente middleware en la tubería
            }
            catch (Exception)
            {
                context.Response.Body = originalBodyStream; // Restaura el stream en caso de excepción
                throw; // Relanza la excepción para que el siguiente middleware la maneje
            }

            var responseTime = DateTime.UtcNow;
            var duration = responseTime - requestTime;

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("Respuesta saliente: {statusCode} en {duration}ms",
                context.Response.StatusCode, duration.TotalMilliseconds);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}