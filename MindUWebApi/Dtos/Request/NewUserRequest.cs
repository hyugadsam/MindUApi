using Dtos.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Request
{
    public class NewUserRequest
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

        [Required]
        [Range(2,3)]
        public EnumRoles RoleId { get; set; }
    }
}
