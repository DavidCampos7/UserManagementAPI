// Add libraries for validation attributes
using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class User
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        required public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MinLength(5, ErrorMessage = "Email must be at least 5 characters long.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        required public string Email { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [MinLength(2, ErrorMessage = "Department must be at least 2 characters long.")]
        required public string Department { get; set; }

    }
}