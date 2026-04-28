using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServices_LFO.Model.Models
{
    public  class VerifyOTPReq
    {
        public string MobileNo { get; set; }
        public string OTP { get; set; }
    }
}
