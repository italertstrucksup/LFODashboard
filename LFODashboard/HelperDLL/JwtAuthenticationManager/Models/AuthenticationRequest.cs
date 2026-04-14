namespace JwtAuthenticationManager.Models
{
    public class AuthenticationRequest
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string AccessType { get; set; }   
    }
}
