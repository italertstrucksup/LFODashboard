namespace AuthServices_LFO.Model.Models
{
    public class TokenResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ReferenceToken { get; set; }
        public string RefreshToken { get; set; }
        public int Validity { get; set; }
    }

}
