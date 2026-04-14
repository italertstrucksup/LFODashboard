using System;
using System.Collections.Generic;
using System.Text;

namespace JwtAuthenticationManager.Models
{
    public class AuthenticationResponse
    {
        public string UserName { get; set; }
        public string JwtToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }


    }


}
