using Dtos.CustomValidations;
using Dtos.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Request
{
    public class UpdateUserRequest
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [EmptyOrMinLength(5)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(2, 3)]
        public EnumRoles RoleId { get; set; }

    }
}
