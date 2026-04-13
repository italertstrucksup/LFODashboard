namespace AuthServices_LFO.Model.Models
{
    public class LoginRequest
    {
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? ReferenceToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
