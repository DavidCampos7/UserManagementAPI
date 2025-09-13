// Controllers/SecureController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/secure")]
    public class SecureController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize] // Requiere token válido
        public IActionResult GetSecureData()
        {
            return Ok(new { message = "Acceso autorizado a datos seguros." });
        }
    }
}
