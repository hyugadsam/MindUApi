using Dtos.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Request
{
    public class NewUserRequest: NewBasicUserRequest
    {
        [Required]
        [Range(2, 3)]
        public EnumRoles RoleId { get; set; }
    }
}
