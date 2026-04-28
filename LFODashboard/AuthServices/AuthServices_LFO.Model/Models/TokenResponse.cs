namespace AuthServices_LFO.Model.Models
{
    public class TokenResponse
    {
        public Guid? UserId { get; set; }
        public string? AccessType { get; set; }
        public string ReferenceToken { get; set; }
        public string RefreshToken { get; set; }
        public int Validity { get; set; }
    }

}
