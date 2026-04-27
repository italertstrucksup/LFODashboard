namespace AuthServices_LFO.Model.Models
{
    
        public class SignupRequest
        {
            public string MobileNo { get; set; }
            public string? Password { get; set; }

      


    }

        public class OTPVerifyRequest   
        {
            public string MobileNo { get; set; }
            public string OTP { get; set; }
            public string Password { get; set; }
            public string OTPType { get; set; }
        }
    
}
