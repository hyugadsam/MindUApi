using DBService.Entities;

namespace DBService.Models
{
    public class LoginResponse
    {
        public Users User { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
