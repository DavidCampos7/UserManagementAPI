using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _issuer = "tu_issuer";
        private readonly string _audience = "tu_audience";
        private readonly string _secretKey = "EsteEsUnSecretoMuySeguroYConMásDe32Caracteres";

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                context.Items["UserId"] = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            await _next(context);
        }

    }
}
