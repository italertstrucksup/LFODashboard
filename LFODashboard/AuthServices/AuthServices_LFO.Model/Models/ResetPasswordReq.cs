using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServices_LFO.Model.Models
{
    public class ResetPasswordReq
    {
        public string MobileNo { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
    }
}
