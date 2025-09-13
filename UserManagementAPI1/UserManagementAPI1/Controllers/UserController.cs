using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;

namespace UserApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly Dictionary<int, User> users = new()
        {
            { 1, new User { Id = 1, Name = "Alice", Email = "alice@example.com", Department = "HR" } },
            { 2, new User { Id = 2, Name = "Bob", Email = "bob@example.com", Department = "IT" } }
        };

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            if (!users.Any())
                return NoContent();

            return Ok(users.Values.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            if (!users.TryGetValue(id, out var user))
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            if (users.ContainsKey(user.Id))
                return Conflict("Ya existe un usuario con ese Id.");

            users[user.Id] = user;
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            if (!users.TryGetValue(id, out var user))
                return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Department = updatedUser.Department;
            users[id] = user;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!users.ContainsKey(id))
                return NotFound();

            users.Remove(id);
            return NoContent();
        }

        [HttpGet("test-error")]
        [AllowAnonymous] // Permite probar el middleware de errores sin autenticación
        public IActionResult ThrowTestError()
        {
            throw new InvalidOperationException("Esto es una excepción de prueba.");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("secure-check")]
        public IActionResult SecureCheck()
        {
            var isAuth = User.Identity?.IsAuthenticated ?? false;
            var name = User.Identity?.Name;
            return Ok(new { isAuth, name });
        }

    }
}
