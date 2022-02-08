using Dtos.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Request
{
    public class NewBasicUserRequest
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

    }
}
