using Dtos.Dtos;

namespace Dtos.Responses
{
    public class UserValidationResponse
    {
        public UserDto User { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
