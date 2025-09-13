// Models/LoginRequest.cs
using System.ComponentModel.DataAnnotations;
namespace UserApi.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        required public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        required public string Password { get; set; }
    }
}
