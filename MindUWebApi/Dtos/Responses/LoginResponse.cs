using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Responses
{
    public class LoginResponse : BasicResponse
    {
        public string Token { get; set; }
        public DateTime Expriation { get; set; }
    }
}
