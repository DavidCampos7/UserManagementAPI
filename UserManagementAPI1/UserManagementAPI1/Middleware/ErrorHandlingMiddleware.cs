namespace UserApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no controlada.");

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    var errorResponse = new { error = "Error interno del servidor." };

                    try
                    {
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    catch (Exception writeEx)
                    {
                        _logger.LogError(writeEx, "Error al escribir la respuesta JSON.");
                    }
                }
                else
                {
                    _logger.LogWarning("No se puede escribir en la respuesta: ya fue iniciada.");
                }
            }
        }
    }
}
