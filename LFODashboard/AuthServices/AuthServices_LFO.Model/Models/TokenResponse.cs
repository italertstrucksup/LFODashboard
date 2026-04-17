namespace AuthServices_LFO.Model.Models
{
    public class TokenResponse
    {
        
        public string ReferenceToken { get; set; }
        public string RefreshToken { get; set; }
        public int Validity { get; set; }
    }

}
